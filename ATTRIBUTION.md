# Attributions

## Mods etudies

Aucun code n'a ete recopie. Les mods ci-dessous ont servi a comprendre les points d'accroche de
l'API du menu Architecte ; les implementations d'Architect Studio sont ecrites a neuf.

Sous licence **MIT**, par **fernyrepos** (auteur : ferny ; C# : Taranchuk, StreetKing) :

- [Colored Categories](https://github.com/fernyrepos/Colored-Categories) - a montre que
  `MainTabWindow_Architect.DoCategoryButton` est le point ou teinter un bouton de categorie.
  Architect Studio y pose un simple prefixe Harmony, la ou l'original emploie un transpileur et un
  remplacement d'atlas.
- [Better Architect Menu](https://github.com/fernyrepos/Better-Architect-Menu) - a servi a
  identifier ses propres caches d'affichage et son extension d'imbrication, avec lesquels
  Architect Studio dialogue par reflexion.

Le texte complet de leur licence est reproduit dans `LICENSE-fernyrepos.txt`
(MIT License, Copyright (c) 2025 fernyrepos).

**Category Manager** (Moriarty) a ete consulte pour comparer les approches de selection d'icone.
Rien n'en a ete repris : ce mod est retire du Workshop, livre sans source ni fichier de licence.
Architect Studio lit les textures deja chargees par RimWorld plutot que de parcourir le disque.

## Integrations par reflexion

Aucun assembly tiers n'est reference a la compilation ; tout est resolu au runtime et le mod
fonctionne si ces mods sont absents.

- **Better Architect Menu** (ferny) - sous-categories, purge des caches d'affichage.
- **Architect Icons** (bymarcin) - recherche et choix de l'icone d'une categorie.
- **Float Sub-Menus** (kathanon) - sous-menus imbriques dans les menus de choix.
