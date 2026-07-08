# Design Pattern

## Projet — Modules complémentaires

---

## Module : Gestion des entrées et sorties

- Permettre de charger les entrées depuis un fichier
- Permettre d'enregistrer la sortie dans un fichier
- Trois formats de fichiers doivent être supportés :
  - Fichier plat (1 ligne = 1 instruction)
  - JSON
  - XML

---

## Module : Modification de drones

- Permettre d'ajouter des pièces (WITH)
- Permettre de retirer des pièces (WITHOUT)
- Permettre de remplacer des pièces (REPLACE)
- Respecter les contraintes de construction des drones

---

## Module : Gestion des instructions de construction

- Permettre l'entrée des instructions de production (GET_OUT_STOCK, ASSEMBLE & INSTALL)
- Gestion du stock des assemblages

---

## Module : Gestion de commandes de ventes

- Ajouter une instruction de commande (ORDER)
- Permettre la sortie de stock de drones déjà construits (SEND)
- Permettre le listing des commandes restantes (LIST_ORDER)

---

## Module : Traçabilité des flux

- Historiser chacun des mouvements de stock (pièce, assemblage, drone)
- Permettre la consultation de l'historique (GET_MOVEMENTS)

---

## Module : Gestion d'un groupe d'usines

- Permettre de gérer plusieurs usines avec des stocks/possibilités d'assemblages différents
- Répartir les instructions sur les différentes usines
- Permettre le transfert de stock entre usines (TRANSFER)
- Permettre de contraindre l'usine cible d'une instruction (IN)
