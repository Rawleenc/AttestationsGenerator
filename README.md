# Générateur d'attestation de sortie dérogatoire

Ce petit logiciel vous permet de générer une attestation de sortie dérogatoire préremplie grâce aux infos que vous aurez fournie.

## Fonctionnement

Lorsque vous lancez le logiciel, vous avez deux menus déroulants et un bouton.  
Le premier menu vous permet de choisir votre profil.  
Le second vous permet de choisir votre motif de sortie.  
Le bouton vous permet de générer l'attestation.

Les profils sont lus depuis un fichier appelé "profiles.json" dans un dossier AttestationsGenerator présent dans vos Documents.  
Vous pouvez générer un squelette d'exemple de ce fichier grâce à l'entrée de menu Option.  
Si vous avez modifié ce fichier alors que le logiciel est ouvert, vous pouvez recharger ce fichier via l'entrée de menu Option.
Ce fichier est chargé à chaque ouverture du logiciel. Attention, veillez à bien respecter le format du fichier. Si vous faites une erreur, le logiciel ne se lancera pas.

L'attestation est téléchargée depuis le site du gouvernement et est directement remplie avec vos informations, la date d'aujourd'hui et l'heure à laquelle vous l'avez générée.

## Installation

Sur Windows, installez ce logiciel via le setup.exe situé dans le dossier `Setup\Release\`.  
Si vous avez déjà installé une version précédente, elle sera normalement remplacée. Cependant si vous obtenez tout de même une erreur lors de l'installation, désinstallez la version précédente.
