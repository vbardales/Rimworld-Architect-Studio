---
mod:        Architect Studio
packageId:  nelim.architectstudio
depot:      Rimworld-Architect-Studio
visibilite: public
detache:    oui
etape:      done
licence:    open
licence_ou: LICENSE-fernyrepos.txt, MIT
vitrine:    complete
teste_le:   2026-09-03
workshop:   3792784018
reste:
  - non_verifie: glisser-deposer d'un membre de groupe, jamais rejoue depuis ses deux correctifs
  - non_verifie: fleches haut/bas depuis leur reecriture en 1.0.2, et a 150% d'echelle
  - non_verifie: categorie forcee sur tout un groupe, et heritage par les membres ajoutes ensuite
  - non_verifie: creation d'une categorie, et son entree dans la configuration des touches
  - non_verifie: option montrant ce que la recherche verrouille encore
session:    local_ea269783-fdb3-4329-83ed-5e4ad5f22536
maj:        2026-09-12, session du mod
---

# Architect Studio — etat

Fiche d'etat, lue par une passe sur tous les mods plutot qu'en interrogeant les fils un a un.
Elle vit a la racine, jamais dans `Mod/`, donc Steam ne la recoit pas.

Les champs deduits du disque le 2026-09-12 ont ete verifies un a un et sont justes. Les trois
que le releve ne pouvait pas remplir sont tranches ici.

- **`etape`** — `done` confirme. Trois versions publiees, 1.0.0 a 1.0.2, vitrine faite, item
  Workshop 3792784018 en ligne.
- **`teste_le`** — le 2026-09-03, date du dernier correctif qui exigeait de voir le mod tourner :
  les fleches haut/bas sortaient de leur bouton des que l'echelle d'interface depassait 100%, ce
  qui ne se constate qu'a l'ecran. La ligne posee d'office par le releve, « jamais vu tourner en
  jeu », etait fausse pour ce mod : `TESTING.md` nomme quatre choses deja vues marcher dans une
  vraie partie, les couleurs de categorie et de sous-categorie, la suppression d'une categorie
  creee ici, et le changement de son parent.
- **`reste`** — les cinq scenarios de `TESTING.md` qui n'ont jamais tourne. Aucun defaut connu
  non corrige, aucune fonctionnalite manquante au premier jet : ce qui reste est du non verifie,
  pas du casse. Le glisser-deposer vient en tete parce qu'il a porte deux bugs distincts et que
  ni l'un ni l'autre correctif n'a ete rejoue depuis.

`TESTING.md` reste la source : il dit pour chaque scenario ce qu'il prouve et a quoi ressemble
son echec. Cette fiche n'en garde que le solde.

Vocabulaire de `licence` : `open` licence explicite, `silent` aucune licence et source morte,
`alive` aucune licence mais source vivante, `forbidden` refus ecrit, `original` rien de repris.
