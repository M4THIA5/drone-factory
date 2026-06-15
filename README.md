# DroneFactory

Projet scolaire ESGI — cours Design Patterns (C#). Application console qui simule une chaîne de production de drones : elle lit des commandes sur l'entrée standard, gère un stock de pièces, et émet les instructions d'assemblage correspondantes.

## Prérequis

- [.NET SDK 10](https://dotnet.microsoft.com/download) (le projet cible `net10.0`).

Vérifier l'installation :

```sh
dotnet --version
```

## Lancer le projet

Depuis la racine du dépôt :

```sh
dotnet run
```

L'application démarre une boucle REPL et attend des commandes sur `stdin`. Saisir une commande par ligne, puis `Ctrl+D` (EOF) pour quitter proprement.

### Compiler sans exécuter

```sh
dotnet build
```

### Exécuter avec un fichier d'entrée

```sh
dotnet run < commandes.txt
```

## Commandes disponibles

| Commande | Description |
|---|---|
| `STOCKS` | Affiche l'intégralité du stock (drones puis pièces). |
| `NEEDED_STOCKS <qty> <Drone>[, <qty> <Drone>...]` | Détaille les pièces nécessaires pour produire une commande, avec un total agrégé. |
| `INSTRUCTIONS <qty> <Drone>[, ...]` | Émet la séquence d'instructions d'assemblage. |
| `VERIFY <qty> <Drone>[, ...]` | Répond `AVAILABLE` ou `UNAVAILABLE` selon l'état du stock. |
| `PRODUCE <qty> <Drone>[, ...]` | Décrémente le stock et produit les drones (`STOCK_UPDATED`). |
| `ADD_TEMPLATE <Nom>, <Piece1>, ..., <PieceN>` | Enregistre un nouveau modèle de drone (`TEMPLATE_ADDED`), utilisable ensuite dans toutes les commandes. |

Drones supportés au démarrage : `DXF-1`, `RDL-1`, `WDS-1`, `DYM-1`. D'autres peuvent être
ajoutés à l'exécution via `ADD_TEMPLATE`.

### Templates de drones (`ADD_TEMPLATE`)

Un template doit contenir exactement une pièce de chaque type (coque, module principal,
générateur, module de déplacement, module de contrôle, système). Il est validé avant
enregistrement :

- le système doit être installable sur le module principal (types du système ⊆ types du core) ;
- le module de contrôle doit être compatible avec le système (au moins un type en commun) ;
- le drone doit appartenir à au moins une catégorie : **Aérien**, **Marin**, **Terrestre** ou
  **Submersible**.

Tout manquement renvoie une erreur `ERROR ...` explicite. Exemple valide :

```
ADD_TEMPLATE MARIN-1, Hull_HG1, Core_CG1, Generator_GG1, Move_MM1, Processor_PG1, System_SG1
TEMPLATE_ADDED
```

### Exemple

```sh
$ dotnet run
INSTRUCTIONS 1 DXF-1
PRODUCING DXF-1
GET_OUT_STOCK 1 Hull_HF1
GET_OUT_STOCK 1 Core_C3D1
GET_OUT_STOCK 1 Generator_GF1
GET_OUT_STOCK 1 Move_MF1
GET_OUT_STOCK 1 Processor_P3D1
INSTALL System_S3D1 Core_C3D1
ASSEMBLE TMP1 Hull_HF1 Generator_GF1
ASSEMBLE TMP2 TMP1 Move_MF1
ASSEMBLE TMP2 Core_C3D1{System_S3D1}
ASSEMBLE [TMP2, Core_C3D1{System_S3D1}] Processor_P3D1
FINISHED DXF-1
```

Le stock initial est de 10 unités pour chaque pièce et chaque système, et 0 drone produit.
