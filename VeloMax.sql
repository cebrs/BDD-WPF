DROP DATABASE velomax;
CREATE DATABASE IF NOT EXISTS  VeloMax;
use VeloMax;

create table if not exists Bicyclette
(No_bicyclette int(3) not null,
Stock_bicyclette int,
nom_bicyclette varchar(20),
grandeur varchar(20),
prix_bicyclette varchar(20),
ligne_produit varchar(20),
date_intro date,
date_discontinuation date,
primary key (No_bicyclette)
);

create table if not exists Pièce
(No_pièce varchar(5) not null,
Stock_pièce int,
description_pièces varchar(50),
prix int,
primary key(No_pièce)
);

create table if not exists Fournisseur
(siret varchar(14) not null,
nom_entreprise varchar(20),
contact_entreprise varchar(20),
adresse_entreprise varchar(100),
libellé int null check (libellé<5),
primary key (siret));

create table if not exists Commande
(No_commande int not null primary key,
date_commande date,
date_livraison date,
prix int
);

create table if not exists Boutique_spécialisé
(No_compagnie int not null auto_increment primary key,
nom_compagnie varchar(20) not null,
adresse_compagnie varchar(100),
telephone_compagnie int(10),
courriel_compagnie varchar(20),
remise int(3));

create table if not exists Fidélio
(No_fidélio int(1) not null,
description_fidélio varchar(20),
cout_fidélio int,
durée int,
rabais int,
primary key(No_fidélio));

create table if not exists Individu
(No_client int not null auto_increment primary key,
nom_client varchar(20) not null,
prenom_client varchar(20),
adresse_client varchar(100),
telephone_client int(10),
courriel_client varchar(20),
fin_fidélio date,
No_fidélio int(1),
constraint FK_individu_fidelio foreign key(No_fidélio)
	references Fidélio (No_fidélio)
	on delete cascade
    on update cascade);

create table if not exists est_composé
(No_pièce varchar(5),
Nombre_nécessaire int,
constraint FK1_compo_pièce foreign key(No_pièce)
	references Pièce (No_pièce)
	ON DELETE cascade
	ON UPDATE cascade,
No_bicyclette int(3),
constraint FK2_compo_pièce foreign key(No_bicyclette)
	references Bicyclette (No_bicyclette)
	ON DELETE cascade
	ON UPDATE cascade,
primary key (No_bicyclette,No_pièce)
);

create table if not exists fournit
(no_catalogue varchar(20),
delai_livraison int,
prix_pièce int,
No_pièce varchar(5),
constraint FK_fournit_pièce foreign key(No_pièce)
	references Pièce (No_pièce)
    ON DELETE cascade
	ON UPDATE cascade,
siret varchar(14),
constraint FK_fournit_fournisseur foreign key(siret)
	references Fournisseur (siret)
    ON DELETE cascade
	ON UPDATE cascade,
primary key (siret,No_pièce)
);

create table if not exists contient_bicyclette
(quantité_bicyclette int ,
No_bicyclette int(3),
constraint FK_contientbi_bicyclette foreign key(No_bicyclette)
	references Bicyclette (No_bicyclette)
    ON DELETE cascade
	ON UPDATE cascade,
No_commande int,
constraint FK_contientbi_commande foreign key(No_commande)
	references Commande (No_commande)
    ON DELETE cascade
	ON UPDATE cascade,
primary key (No_bicyclette,No_commande)
);

create table if not exists contient_pièces
(quantité_pièces int ,
No_commande int,
constraint FK_contientpi_commande foreign key(No_commande)
	references Commande (No_commande)
    ON DELETE cascade
	ON UPDATE cascade,
No_pièce varchar(5),
constraint FK_contientpi_piece foreign key(No_pièce)
	references Pièce (No_pièce)
    ON DELETE cascade
	ON UPDATE cascade,
primary key (No_pièce,No_commande)
);

create table if not exists commande_Client
(No_commande int,
constraint FK_commandeC_commande foreign key(No_commande)
	references Commande (No_commande)
    ON DELETE cascade
	ON UPDATE cascade,
No_client int,
constraint FK_commandeC_individu foreign key(No_client)
	references Individu (No_client)
    ON DELETE cascade
	ON UPDATE cascade,
primary key (No_client,No_commande)
);

create table if not exists commande_Boutique
(No_commande int,
constraint FK_commandeB_commande foreign key(No_commande)
	references Commande (No_commande)
    ON DELETE cascade
	ON UPDATE cascade,
No_compagnie int,
constraint FK_commandeB_boutique foreign key(No_compagnie)
	references Boutique_spécialisé (No_compagnie)
    ON DELETE cascade
	ON UPDATE cascade,
primary key (No_compagnie,No_commande)
);

-- insert Fidélio--
insert into `velomax`.`fidélio` (`No_fidélio`,`description_fidélio`,`cout_fidélio`,`durée`,`rabais`) values (1,'Fidélio',15,1,5);
insert into `velomax`.`fidélio` (`No_fidélio`,`description_fidélio`,`cout_fidélio`,`durée`,`rabais`) values (2,'Fidélio Or',25,2,8);
insert into `velomax`.`fidélio` (`No_fidélio`,`description_fidélio`,`cout_fidélio`,`durée`,`rabais`) values (3,'Fidélio Platine',60,1,10);
insert into `velomax`.`fidélio` (`No_fidélio`,`description_fidélio`,`cout_fidélio`,`durée`,`rabais`) values (4,'Fidélio Max',100,1,12);

-- insert Bicyclette--
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(101,10,'Kilimandjaro','Adultes','569','VTT','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(102,10,'NorthPole','Adultes','329','VTT','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(103,10,'MontBlanc','Adultes','399','VTT','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(104,10,'Hooligan','Jeunes','199','VTT','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(105,10,'Orléans','Hommes','229','Vélo de course','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(106,10,'Orléans','Femmes','229','Vélo de course','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(107,10,'BlueJay','Hommes','349','Vélo de course','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(108,10,'BlueJay','Dames','349','Vélos de course','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(109,10,'TrailExplorer','Filles','129','Classique','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(110,10,'TrailExplorer','Garçons','129','Classique','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(111,10,'Night Hawk','Jeunes','189','Classique','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(112,10,'Tierra Verde','Hommes','199','Classique','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(113,10,'Tierra Verde','Dames','199','Classique','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(114,10,'Mud Zinger I','Jeunes','279','BMX','2000--04--01','2000--12--05');
insert into `velomax`.`bicyclette` (`No_bicyclette`,`Stock_bicyclette`,`nom_bicyclette`,`grandeur`,`prix_bicyclette`,`ligne_produit`,`date_intro`,`date_discontinuation`)values(115,10,'Mud Zinger II','Adultes','359','BMX','2000--04--01','2000--12--05');

-- insert pièces
-- Cadre--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C32',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C34',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C76',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C43',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C44f',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C43f',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C01',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C02',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C15',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C87',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C87f',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C25',10,'Cadre',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('C26',10,'Cadre',49);
-- Guidon--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('G7',10,'Guidon',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('G9',10,'Guidon',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('G12',10,'Guidon',49);
-- Freins--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('F3',10,'Freins',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('F9',10,'Freins',49);
-- Selle--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S88',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S37',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S35',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S02',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S03',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S36',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S34',10,'Selle',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S87',10,'Selle',49);
-- Dérailleur Avant--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV133',10,'Dérailleur Avant',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV17',10,'Dérailleur Avant',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV87',10,'Dérailleur Avant',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV57',10,'Dérailleur Avant',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV15',10,'Dérailleur Avant',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV41',10,'Dérailleur Avant',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DV132',10,'Dérailleur Avant',49);
-- Dérailleur Arrière--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DR56',10,'Dérailleur Arrière',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DR87',10,'Dérailleur Arrière',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DR86',10,'Dérailleur Arrière',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DR23',10,'Dérailleur Arrière',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DR76',10,'Dérailleur Arrière',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('DR52',10,'Dérailleur Arrière',49);
-- Roue Avant--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R45',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R48',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R12',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R19',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R1',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R11',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R44',10,'Roue',49);
-- Roue Arrière--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R46',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R47',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R32',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R18',10,'Roue',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R2',10,'Roue',49);
-- Réflécteurs--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R02',10,'Réflecteurs',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R09',10,'Réflecteurs',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('R10',10,'Réflecteurs',49);
-- Pédalier--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('P12',10,'Pédalier',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('P34',10,'Pédalier',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('P1',10,'Pédalier',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('P15',10,'Pédalier',49);
-- Ordinateur--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('O2',10,'Odinateur',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('O4',10,'Odinateur',49);
-- Panier--
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S01',10,'Panier',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S74',10,'Panier',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S73',10,'Panier',49);
insert into `velomax`.`pièce` (`No_pièce`,`Stock_pièce`,`description_pièces`, `prix`) values ('S05',10,'Panier',49);

-- est composé--
-- Kilimandjaro--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C32',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G7',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F3',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S88',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV133',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR56',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R45',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R46',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P12',101);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'O2',101);
-- NorthPole--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C34',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G7',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F3',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S88',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV17',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR87',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R48',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R47',102);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P12',102);
-- MontBlanc--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C76',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G7',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F3',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S88',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV17',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR87',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R48',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R47',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P12',103);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'O2',103);
-- Hooligan--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C76',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G7',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F3',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S88',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV87',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR86',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R12',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R32',104);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P12',104);
-- Orélans Hommes--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C43',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G9',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S37',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV57',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR86',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R19',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R18',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R02',105);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P34',105);
-- Orléans dames--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C44f',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G9',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S35',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV57',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR86',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R19',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R18',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R02',106);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P34',106);
-- BlueJay Hommes--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C43',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G9',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S37',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV57',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR87',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R19',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R18',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R02',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P34',107);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'O4',107);
-- BlueJay Dames--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C43f',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G9',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S35',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV57',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR87',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R19',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R18',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R02',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P34',108);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'O4',108);
-- Trail Explorer Filles--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C01',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G12',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S02',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R1',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R2',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R09',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P1',109);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S01',109);
-- Trail Explorer Garçons--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C02',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G12',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S03',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R1',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R2',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R09',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P1',110);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S05',110);
-- Night Hawk--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C15',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G12',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S36',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV15',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR23',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R11',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R12',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R10',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P15',111);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S74',111);
-- Tierra Verde Hommes--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C87',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G12',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S36',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV41',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR23',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R11',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R12',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R10',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P15',112);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S74',112);
-- Tierra Verde Dames--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C87f',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G12',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F9',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S34',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV41',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR23',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R11',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R12',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R10',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P15',113);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S74',113);
-- Mud Zinger I Jeunes--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C25',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G7',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F3',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S87',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV132',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR52',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R44',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R47',114);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P12',114);
-- Mud Zinger II Adultes--
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'C26',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'G7',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'F3',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'S87',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DV133',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'DR52',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R44',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'R47',115);
insert into `velomax`.`est_composé` (`Nombre_nécessaire`,`No_pièce`,`No_bicyclette`) values (1,'P12',115);

-- Clients
insert into `velomax`.`individu` (`nom_client`,`prenom_client`,`adresse_client`,`telephone_client`,`courriel_client`) values ('BAUDU','thomas','18 rue Malakoff 92600 Asnières','0782489070','thombaudu@gmail.com');
insert into `velomax`.`individu` (`nom_client`,`prenom_client`,`adresse_client`,`telephone_client`,`courriel_client`, `fin_fidélio`,`no_fidélio`)values ('Portman','Nathalie','Dunkerque','0782489070','nportman@outlook.com', '2021-05-20', 1);
insert into `velomax`.`individu` (`nom_client`,`prenom_client`,`adresse_client`,`telephone_client`,`courriel_client`, `fin_fidélio`,`no_fidélio`)values ('Martin','Pascal','Paris','0782489070','pp@outlook.com', '2021-12-20', 3);
insert into `velomax`.`individu` (`nom_client`,`prenom_client`,`adresse_client`,`telephone_client`,`courriel_client`, `fin_fidélio`,`no_fidélio`)values ('Barras','Evan','6 allée des épines','0646121169','ev.brs@outlook.com', '2022-05-20', 4);
insert into `velomax`.`individu` (`nom_client`,`prenom_client`,`adresse_client`,`telephone_client`,`courriel_client`, `fin_fidélio`,`no_fidélio`)values ('Jackson','Mickael','Los Angeles','0782489070','jjjj@gmail.com', '2021-07-28', 2);

-- Boutique Spé
insert into `velomax`.`boutique_spécialisé` (`nom_compagnie`,`adresse_compagnie`,`telephone_compagnie`,`courriel_compagnie`,`remise`) values ('BARRAS Célia','A dash les bains','0000000000','cece78180@gmail.com',15);
insert into `velomax`.`boutique_spécialisé` (`nom_compagnie`,`adresse_compagnie`,`telephone_compagnie`,`courriel_compagnie`,`remise`) values ('Carrefour','Montigny-le-Bretonneux','0000000000','sav@carrefour.fr',15);
insert into `velomax`.`boutique_spécialisé` (`nom_compagnie`,`adresse_compagnie`,`telephone_compagnie`,`courriel_compagnie`,`remise`) values ('VeloPlus','1 rue de la pluie Courbevoie','0139440789','veloPLUS@neuf.fr',15);

-- Fournisseur
insert into `velomax`.`fournisseur` (`siret`,`nom_entreprise`,`contact_entreprise`,`adresse_entreprise`,`libellé`) values ('30613890001294','Décathlon','0969363210',"4 Boulevard De Mons Ascq, 59650 Villeneuve D'Ascq, France",2);
insert into `velomax`.`fournisseur` (`siret`,`nom_entreprise`,`contact_entreprise`,`adresse_entreprise`,`libellé`) values ('30613645701294','Alltricks','8746534678',"3 rue de la lune, 92345 Courbevoie, France",4);
insert into `velomax`.`fournisseur` (`siret`,`nom_entreprise`,`contact_entreprise`,`adresse_entreprise`,`libellé`) values ('45313890001294','Summun Bike','0969363210',"11 Rue des Tilleuls, 78960 Voisins-le-Bretonneux, France",1);
insert into `velomax`.`fournisseur` (`siret`,`nom_entreprise`,`contact_entreprise`,`adresse_entreprise`,`libellé`) values ('45313890078904','Bicloune','0148054775',"65 Avenue Daumesnil, 75012 Paris, France",3);
insert into `velomax`.`fournisseur` (`siret`,`nom_entreprise`,`contact_entreprise`,`adresse_entreprise`,`libellé`) values ('78913890078904','Velotority','0158513578',"1 Rue du Liban, 75020 Paris, France",3);
insert into `velomax`.`fournisseur` (`siret`,`nom_entreprise`,`contact_entreprise`,`adresse_entreprise`,`libellé`) values ('23413890078904','EN SELLE MARCEL','0130211077',"12 Parvis Colonel Arnaud Beltrame, 78000 Versailles, France",3);

-- fournit 
insert into `velomax`.`fournit`(`no_catalogue`,`delai_livraison`, `prix_pièce`, `No_pièce`, `siret`) values ('5467YG', 5, 32, 'C01','30613890001294');
insert into `velomax`.`fournit`(`no_catalogue`,`delai_livraison`, `prix_pièce`, `No_pièce`, `siret`) values ('GFE7SJH', 7, 36, 'C02','30613890001294');
insert into `velomax`.`fournit`(`no_catalogue`,`delai_livraison`, `prix_pièce`, `No_pièce`, `siret`) values ('2678TNJ', 8, 34, 'C15','30613890001294');
insert into `velomax`.`fournit`(`no_catalogue`,`delai_livraison`, `prix_pièce`, `No_pièce`, `siret`) values ('9876JS', 5, 156, 'R18','30613890001294');
insert into `velomax`.`fournit`(`no_catalogue`,`delai_livraison`, `prix_pièce`, `No_pièce`, `siret`) values ('7897YG', 5, 56, 'C01','45313890001294');
insert into `velomax`.`fournit`(`no_catalogue`,`delai_livraison`, `prix_pièce`, `No_pièce`, `siret`) values ('B897YG', 5, 56, 'C01','78913890078904');

-- Commande
insert into `velomax`.`commande`(`no_commande`,`date_commande`, `date_livraison`, `prix`) values (1,'2021-05-19', '2021-05-26', 667);
insert into `velomax`.`contient_bicyclette`(`quantité_bicyclette`,`no_bicyclette`, `no_commande`) values (1,101, 1);
insert into `velomax`.`contient_pièces`(`quantité_pièces`,`no_commande`, `no_pièce`) values (2,1, 'C01');
insert into `velomax`.`commande_client`(`no_commande`,`no_client`) values (1,1);

insert into `velomax`.`commande`(`no_commande`,`date_commande`, `date_livraison`, `prix`) values (2,'2021-05-19', '2021-05-26', 456);
insert into `velomax`.`contient_bicyclette`(`quantité_bicyclette`,`no_bicyclette`, `no_commande`) values (3,104, 2);
insert into `velomax`.`contient_pièces`(`quantité_pièces`,`no_commande`, `no_pièce`) values (2,2, 'C43f');
insert into `velomax`.`commande_boutique`(`no_commande`,`no_compagnie`) values (2,3);

# donne les droits à l'utilisateur
create user 'bozo'@'localhost' identified by 'bozo';
grant select on velomax.* to 'bozo'@'localhost';


