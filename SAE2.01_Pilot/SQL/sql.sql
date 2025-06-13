-- SQL/test_setup.sql (Revised)

-- The user 'battigm' must have privileges to CREATE/DROP SCHEMAS.
-- If 'battigm' cannot drop schemas, a superuser must grant it:
-- GRANT CREATE ON DATABASE your_database_name TO battigm;

-- 1. Drop the *test schema* completely. This removes all tables inside it.
-- This is the most reliable way to ensure a clean slate.
DROP SCHEMA IF EXISTS test CASCADE;
CREATE SCHEMA test AUTHORIZATION benardax;
SET search_path TO test;

-- ==============================================================
-- Table : CATEGORIE
-- ==============================================================
CREATE TABLE CATEGORIE (
    NUMCATEGORIE       SERIAL             NOT NULL,
    LIBELLECATEGORIE   VARCHAR(30)        NULL,
    CONSTRAINT PK_CATEGORIE PRIMARY KEY (NUMCATEGORIE)
);

-- ==============================================================
-- Table : COMMANDE
-- ==============================================================
CREATE TABLE COMMANDE (
    NUMCOMMANDE        SERIAL             NOT NULL,
    NUMEMPLOYE         INT4               NOT NULL,
    NUMTRANSPORT       INT4               NOT NULL,
    NUMREVENDEUR       INT4               NOT NULL,
    DATECOMMANDE       DATE               NULL,
    DATELIVRAISON      DATE               NULL,
    PRIXTOTAL          DECIMAL            NULL,
    CONSTRAINT PK_COMMANDE PRIMARY KEY (NUMCOMMANDE)
);

-- ==============================================================
-- Table : COULEUR
-- ==============================================================
CREATE TABLE COULEUR (
    NUMCOULEUR         SERIAL             NOT NULL,
    LIBELLECOULEUR     VARCHAR(30)        NULL,
    CONSTRAINT PK_COULEUR PRIMARY KEY (NUMCOULEUR)
);

-- ==============================================================
-- Table : COULEURPRODUIT
-- ==============================================================
CREATE TABLE COULEURPRODUIT (
    NUMPRODUIT         INT4               NOT NULL,
    NUMCOULEUR         INT4               NOT NULL,
    CONSTRAINT PK_COULEURPRODUIT PRIMARY KEY (NUMPRODUIT, NUMCOULEUR)
);

-- ==============================================================
-- Table : EMPLOYE
-- ==============================================================
CREATE TABLE EMPLOYE (
    NUMEMPLOYE         SERIAL             NOT NULL,
    NUMROLE            INT4               NOT NULL,
    NOM                VARCHAR(30)        NULL,
    PRENOM             VARCHAR(30)        NULL,
    LOGIN              VARCHAR(30)        NULL,
    CONSTRAINT PK_EMPLOYE PRIMARY KEY (NUMEMPLOYE)
);

-- ==============================================================
-- Table : MODETRANSPORT
-- ==============================================================
CREATE TABLE MODETRANSPORT (
    NUMTRANSPORT       SERIAL             NOT NULL,
    LIBELLETRANSPORT   VARCHAR(30)        NULL,
    CONSTRAINT PK_MODETRANSPORT PRIMARY KEY (NUMTRANSPORT)
);

-- ==============================================================
-- Table : PRODUIT
-- ==============================================================
CREATE TABLE PRODUIT (
    NUMPRODUIT         SERIAL             NOT NULL,
    NUMTYPEPOINTE      INT4               NOT NULL,
    NUMTYPE            INT4               NOT NULL,
    CODEPRODUIT        CHAR(5)            UNIQUE, -- Added UNIQUE constraint for CODEPRODUIT
    NOMPRODUIT         VARCHAR(30)        NULL,
    PRIXVENTE          DECIMAL            NULL,
    QUANTITESTOCK      INT4               NULL,
    DISPONIBLE         BOOL               NULL,
    CONSTRAINT PK_PRODUIT PRIMARY KEY (NUMPRODUIT)
);

-- ==============================================================
-- Table : PRODUITCOMMANDE
-- ==============================================================
CREATE TABLE PRODUITCOMMANDE (
    NUMCOMMANDE        INT4               NOT NULL,
    NUMPRODUIT         INT4               NOT NULL,
    QUANTITECOMMANDE   INT4               NULL,
    PRIX               DECIMAL            NULL,
    CONSTRAINT PK_PRODUITCOMMANDE PRIMARY KEY (NUMCOMMANDE, NUMPRODUIT)
);

-- ==============================================================
-- Table : REVENDEUR
-- ==============================================================
CREATE TABLE REVENDEUR (
    NUMREVENDEUR       SERIAL             NOT NULL,
    RAISONSOCIALE      VARCHAR(30)        NULL,
    ADRESSERUE         VARCHAR(30)        NULL,
    ADRESSECP          CHAR(5)            NULL,
    ADRESSEVILLE       VARCHAR(30)        NULL,
    CONSTRAINT PK_REVENDEUR PRIMARY KEY (NUMREVENDEUR)
);

-- ==============================================================
-- Table : ROLE
-- ==============================================================
CREATE TABLE ROLE (
    NUMROLE            SERIAL             NOT NULL,
    LIBELLEROLE        VARCHAR(30)        NULL,
    CONSTRAINT PK_ROLE PRIMARY KEY (NUMROLE)
);

-- ==============================================================
-- Table : TYPE
-- ==============================================================
CREATE TABLE TYPE (
    NUMTYPE            SERIAL             NOT NULL,
    NUMCATEGORIE       INT4               NOT NULL,
    LIBELLETYPE        VARCHAR(30)        NULL,
    CONSTRAINT PK_TYPE PRIMARY KEY (NUMTYPE)
);

-- ==============================================================
-- Table : TYPEPOINTE
-- ==============================================================
CREATE TABLE TYPEPOINTE (
    NUMTYPEPOINTE      SERIAL             NOT NULL,
    LIBELLETYPEPOINTE  VARCHAR(30)        NULL,
    CONSTRAINT PK_TYPEPOINTE PRIMARY KEY (NUMTYPEPOINTE)
);

-- Foreign Keys (inchangées)
ALTER TABLE COMMANDE
    ADD CONSTRAINT FK_COMMANDE_LIVRERPAR_MODETRAN FOREIGN KEY (NUMTRANSPORT)
      REFERENCES MODETRANSPORT (NUMTRANSPORT)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE COMMANDE
    ADD CONSTRAINT FK_COMMANDE_PASSERPAR_EMPLOYE FOREIGN KEY (NUMEMPLOYE)
      REFERENCES EMPLOYE (NUMEMPLOYE)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE COMMANDE
    ADD CONSTRAINT FK_COMMANDE_PASSERPOU_REVENDEU FOREIGN KEY (NUMREVENDEUR)
      REFERENCES REVENDEUR (NUMREVENDEUR)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE COULEURPRODUIT
    ADD CONSTRAINT FK_COULEURP_COULEURPR_PRODUIT FOREIGN KEY (NUMPRODUIT)
      REFERENCES PRODUIT (NUMPRODUIT)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE COULEURPRODUIT
    ADD CONSTRAINT FK_COULEURP_COULEURPR_COULEUR FOREIGN KEY (NUMCOULEUR)
      REFERENCES COULEUR (NUMCOULEUR)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE EMPLOYE
    ADD CONSTRAINT FK_EMPLOYE_LIE_ROLE FOREIGN KEY (NUMROLE)
      REFERENCES ROLE (NUMROLE)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PRODUIT
    ADD CONSTRAINT FK_PRODUIT_APPARTIEN_TYPE FOREIGN KEY (NUMTYPE)
      REFERENCES TYPE (NUMTYPE)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PRODUIT
    ADD CONSTRAINT FK_PRODUIT_EXISTE_TYPEPOIN FOREIGN KEY (NUMTYPEPOINTE)
      REFERENCES TYPEPOINTE (NUMTYPEPOINTE)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PRODUITCOMMANDE
    ADD CONSTRAINT FK_PRODUITC_PRODUITCO_COMMANDE FOREIGN KEY (NUMCOMMANDE)
      REFERENCES COMMANDE (NUMCOMMANDE)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE PRODUITCOMMANDE
    ADD CONSTRAINT FK_PRODUITC_PRODUITCO_PRODUIT FOREIGN KEY (NUMPRODUIT)
      REFERENCES PRODUIT (NUMPRODUIT)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE TYPE
    ADD CONSTRAINT FK_TYPE_FAITPARTI_CATEGORI FOREIGN KEY (NUMCATEGORIE)
      REFERENCES CATEGORIE (NUMCATEGORIE)
      ON DELETE RESTRICT ON UPDATE RESTRICT;

-- ==============================================================
-- Données Initiales
-- ==============================================================

-- Rôles
INSERT INTO ROLE (LIBELLEROLE) VALUES ('Commercial'), ('Responsable de production');

-- Employés
INSERT INTO EMPLOYE (NUMROLE, NOM, PRENOM, LOGIN) VALUES
(1, 'Battig', 'Morgan', 'battigm'),
(2, 'Benard', 'Axel', 'benardax');

-- Modes de transport
INSERT INTO MODETRANSPORT (LIBELLETRANSPORT) VALUES ('UPS'), ('Chronopost'), ('Relais');

-- Revendeurs
INSERT INTO REVENDEUR (RAISONSOCIALE, ADRESSERUE, ADRESSECP, ADRESSEVILLE) VALUES
('Papeterie du Centre', '12 rue des Lilas', '75001', 'Paris'),
('Fournitures Pro', '45 avenue Gambetta', '69002', 'Lyon'),
('Bureau & Style', '3 place de la République', '31000', 'Toulouse');

-- Catégories
INSERT INTO CATEGORIE (LIBELLECATEGORIE) VALUES
('Bureau'), ('École'), ('Loisir'), ('Haute écriture');

-- Types
INSERT INTO TYPE (NUMCATEGORIE, LIBELLETYPE) VALUES
(1, 'Billes'), (1, 'Roller gel'), (1, 'Roller liquide'), (1, 'Plume feutre'), (3, 'Couleur fun');

-- Types de pointes
INSERT INTO TYPEPOINTE (LIBELLETYPEPOINTE) VALUES
('Pointe fine'), ('Pointe moyenne'), ('Pointe épaisse');

-- Couleurs
INSERT INTO COULEUR (LIBELLECOULEUR) VALUES ('Bleu'), ('Noir'), ('Vert'), ('Rouge');