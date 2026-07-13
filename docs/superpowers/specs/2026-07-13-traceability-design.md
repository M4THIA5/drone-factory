# Module : Traçabilité des flux — Design

Date : 2026-07-13
Statut : approuvé

## But

Historiser chaque mouvement de stock (entrée / sortie de pièces et de drones) et
permettre sa consultation via la commande `GET_MOVEMENTS`. Cinquième module de
`Liste_module.md`.

## Décisions

- **Format de sortie** (`GET_MOVEMENTS`) : une ligne par mouvement, dans l'ordre
  chronologique :

  ```
  <SOURCE> <IN|OUT> <qty> <name>
  ```

  `SOURCE` = commande à l'origine du mouvement (`PRODUCE`, `RECEIVE`, `SEND`).
  Historique vide → `NO_MOVEMENT`.

- **Pattern** : Observer (5ᵉ pattern du projet, nouveau — pas une réutilisation).
  `Stock` est le *Subject* ; `MovementLog` est l'*Observer*.

- **Portée** : le `Stock` actuel ne gère que pièces + drones (pas de stock
  d'assemblages — c'est le module « Gestion des instructions de construction »,
  non implémenté). La traçabilité couvre donc pièces et drones.

## Résolution de la tension Observer ↔ source

L'Observer implique que `Stock` émet les mouvements, mais `Stock` ne connaît pas
la commande à l'origine. La cause est intrinsèque à l'appel de mutation : la
`source` est donc passée en paramètre des méthodes de mutation. `Stock` reste le
*Subject*, la `source` fait simplement partie du payload `Movement`.

## Fichiers

### Nouveaux (`Movements/`)

- `Movement.cs`
  - `enum MovementDirection { In, Out }`
  - `record Movement(string Source, MovementDirection Direction, int Qty, string Name)`
  - Pas d'horodatage → sortie déterministe (golden tests). Pas de champ « kind » :
    le nom distingue pièce et drone.
- `IStockObserver.cs` — `void OnMovement(Movement m);` (contrat Observer).
- `MovementLog.cs` — implémente `IStockObserver` ; `List<Movement>` ordonnée ;
  expose `IReadOnlyList<Movement> Movements`.

### Modifiés

- `Stock.cs`
  - `List<IStockObserver> _observers` + `Subscribe(IStockObserver)` + `Notify(...)`
    privé.
  - Les 4 méthodes de mutation reçoivent un paramètre `string source`. Direction
    et sens sont intrinsèques : `AddPiece`/`AddDrone` = `In`, `ConsumePiece`/
    `ConsumeDrone` = `Out`. Chacune notifie les observers **sauf si `qty == 0`**
    (un no-op n'est pas un mouvement — cas `ADD_TEMPLATE` qui appelle
    `AddDrone(name, 0)`).
  - Le seeding du constructeur reste des écritures directes du dictionnaire → le
    stock initial n'est pas historisé (voulu).
- Appelants passant la source :
  - `ProduceCommand` → `"PRODUCE"`
  - `ReceiveCommand` → `"RECEIVE"`
  - `SendCommand` → `"SEND"`
  - `AddTemplateCommand` → `"ADD_TEMPLATE"` (qty 0 → non historisé)
- `Commands/GetMovementsCommand.cs` — `ICommand`, `MovementLog` injecté, sans
  argument. Affiche chaque mouvement ou `NO_MOVEMENT`.
- `Program.cs` — `var log = new MovementLog(); stock.Subscribe(log);` et
  enregistre `["GET_MOVEMENTS"] = new GetMovementsCommand(log)`.

## Vérification

Pas de projet de test → golden manuel : script de commandes sur `stdin`, diff de
`stdout`. Cas couverts : PRODUCE, RECEIVE, SEND, ADD_TEMPLATE (pas de mouvement
0), historique vide (`NO_MOVEMENT`).

## YAGNI (hors périmètre)

- Pas de filtre/argument sur `GET_MOVEMENTS`.
- Pas de champ « kind ».
- Pas d'horodatage.
