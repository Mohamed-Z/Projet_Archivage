# Projet_Archivage
ASP .NET MVC Archivage

  

instruction
avant de lancer le projet merci de supprimer la base de donnees et relance la migration 
	veuillez verifier la chaine de connection (connectionString) dans le fichier web.config
	si vous utilisez SQLEXPRESS merci d'ajoueter /SQLEXPRESS apres localhost dans le chaine de connection


apres vous lancez la migration en utilisant la commande 
Update-Database -TargetMigration:"InitialCreate"

PS : le fichier le migration et deja inclu dans cette version de projet
