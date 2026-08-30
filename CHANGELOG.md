# Journal des modifications

Format inspiré de [Keep a Changelog](https://keepachangelog.com/fr/1.1.0/).
Ce fichier sert au dépôt et à rédiger les notes de version Steam ; RimWorld ne l'affiche pas en jeu.

## [1.0.0] — non publié

À la publication : créer le tag `v1.0.0` et la release GitHub correspondante.

Première version. RimWorld 1.6.

### Groupes de menus déroulants

- Créer un groupe, y ranger des bâtiments, en retirer.
- Ordonner les membres d'un groupe, par glisser-déposer ou par flèches haut/bas.
- Imposer une catégorie à un groupe entier : ses membres y sont déplacés, et ceux ajoutés ensuite suivent.
- Menu en grille ou en liste, et choix de la source des icônes.
- Supprimer un groupe. Ceux fournis par un autre mod sont dissous et retirés de la liste, puisque leur def revient à chaque démarrage ; un bouton les restaure.
- Avertissement quand un groupe s'étale sur plusieurs catégories, cas où le jeu en fait silencieusement plusieurs boutons.

### Catégories et sous-catégories

- Créer une catégorie, ou une sous-catégorie quand Better Architect Menu est présent.
- Réordonner par flèches haut/bas, au sein de la fratrie.
- Changer le libellé, la couleur et l'icône de n'importe quelle catégorie, y compris celles du jeu de base.
- Sélecteur d'icônes parcourant celles déjà chargées par les mods actifs.
- Catégories sans contenu grisées, avec le compte des bâtiments.

### Divers

- Bouton d'accès dans la fenêtre Architecte, plus une entrée dans les réglages du mod.
- Raccourci clavier non assigné par défaut, à définir soi-même.
- Récapitulatif des intégrations détectées dans les réglages.
- Remise à zéro globale, et remises à zéro ciblées par écran.
- Français et anglais.

### Notes

- Rien n'est écrit dans les defs sur disque : tout est enregistré dans les réglages du mod et réappliqué au démarrage.
- Intégrations facultatives, résolues par réflexion : Better Architect Menu, Architect Icons, Float Sub-Menus, Searchable Menus. Aucune n'est requise.
