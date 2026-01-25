Delete from Athletes ;
Delete from Competitions;
Delete from Gara;
Delete from GaraParams;
Delete from GaraTotal;
Delete from Messages;
Delete from PanelJudge;
Delete from Participants;
Delete from Category;
Delete from GaraRolskanet;

DROP TABLE IF EXISTS Rolskanet

WHERE Manifestation IS NULL
   OR TRIM(Manifestation) = '';


DROP VIEW IF EXISTS BDD_MEDAILLE_DANSE;
DROP VIEW IF EXISTS sequence_danse;
DROP VIEW IF EXISTS total_score;
DROP VIEW IF EXISTS medailles_danse;

DROP VIEW IF EXISTS BDD_MEDAILLE_FREE;
DROP VIEW IF EXISTS Ext_medaille_free;
DROP VIEW IF EXISTS Base_medaille_free;


INSERT INTO GaraRolskanet(Type,Filiere,Categorie)
SELECT Type,
       Filière,
       [Épreuve]
FROM Rolskanet
GROUP BY Type,
         Filière,
         [Épreuve];


INSERT into Athletes(Name,Societa,Country,Region,ID_Specialita,Num_Licence)
SELECT
    Rolskanet.Groupe as Name,
    "Nom Club" as Societa,
    'FRA' as Country,
    [N°Ligue - Nom Ligue] as Region,
    Specialites.ID_Specialita as ID_Specialita,
    Rolskanet.[Épreuve] as Num_Licence
From Rolskanet,Specialites
WHERE
    upper(Rolskanet.type)=upper(Specialites.Libellé)
  AND Rolskanet.Groupe<>'-'
Group by Rolskanet.Type, Rolskanet.[Épreuve],Rolskanet.Groupe ;


Insert into Competitions (Name,Place,Nation,Date,DateEnd,CompetitionType,StatusEvent)
select 
Manifestation as Name,
[Commune manifestation] as Place,
'FRA' as Nation,
[Date de début],
[Date de fin],
'National competition' as type,
0 as Status
From Rolskanet
Group by Manifestation;

INSERT INTO Category(ID_Category,Name,Ordine)
SELECT
    ID_Category,
    Name,
    Ordine
FROM Categories;

INSERT INTO Athletes
(Name, Societa, Country, Region, ID_Specialita, Num_Licence)
SELECT
    Nom || ' - ' || Prenom AS Name,
    [Nom Club] AS Societa,
    'FRA' AS Country,
    [N°Ligue - Nom Ligue] AS Region,
    Specialites.ID_Specialita AS ID_Specialita,
    [Numéro de Licence] AS Num_Licence
FROM Rolskanet
    JOIN Specialites
ON upper(Rolskanet.type) = upper(Specialites.Libellé)
WHERE Rolskanet.Groupe = '-';


INSERT INTO GaraParams(ID_GaraParams,Name,PLace,Date,DateEnd,ID_Segment,ID_Specialita,ID_Category,Partecipants,Sex,Completed,LastPartecipant,NumJudges,Factor,SkatersPerGroup,ID_Competition)
select 
GaraRolskanet.IdGara,
Categories.Filiere || ' ' || GaraRolskanet.Categorie,
Rolskanet.[Commune manifestation] as Place,
[Date de début],
[Date de fin],
CASE 
    WHEN categories.ID_Category in (21,23,25,27,29) AND Specialites.ID_Specialita in (10,11) then 2
    WHEN Categories.ID_Category<50 THEN Matrix.ID_Segment
    WHEN (Categories.ID_Category>=50 AND (Specialites.ID_Specialita=1 OR Specialites.ID_Specialita=2)) THEN 2
    WHEN (Categories.ID_Category>=50 AND (Specialites.ID_Specialita=5 OR Specialites.ID_Specialita=6)) THEN 4
    END as ID_Segment,
Specialites.ID_Specialita,
CASE 
    WHEN (Categories.ID_Category>=50 AND (Specialites.ID_Specialita=5 OR Specialites.ID_Specialita=6)) THEN 25
    ELSE Categories.ID_Category
END AS ID_Category,
count(DISTINCT Rolskanet.[Numéro de licence]) as Participants,
Sexe,
'N' as Completed,
'0',
'3',
'1',
'6',
Competitions.ID_Competition      
From GaraRolskanet
INNER JOIN Specialites ON upper(GaraRolskanet.type)=upper(Specialites.Libellé)
INNER JOIN Categories ON upper(Categories.Categorie)=upper(GaraRolskanet.Categorie) and upper(Categories.Filiere)=upper(GaraRolskanet.Filiere)
INNER JOIN Matrix ON Matrix.ID_Specialita=Specialites.ID_Specialita AND Matrix.ID_Category=Categories.ID_Category
INNER JOIN Rolskanet ON upper(Rolskanet.Type)=upper(GaraRolskanet.Type) and upper(Rolskanet.Filière)=upper(GaraRolskanet.Filiere) AND upper(Rolskanet.[Épreuve])=upper(GaraRolskanet.Categorie)
INNER JOIN Competitions ON upper(Competitions.[Name])=upper(Rolskanet.Manifestation )
WHERE Specialites.ID_Specialita in (1,2,5,6,10,11)
GROUP BY Competitions.ID_Competition,GaraRolskanet.IdGara,ID_Segment;

INSERT INTO GaraParams(ID_GaraParams,Name,PLace,Date,DateEnd,ID_Segment,ID_Specialita,ID_Category,Partecipants,Sex,Completed,LastPartecipant,NumJudges,Factor,SkatersPerGroup,ID_Competition)
select 
GaraRolskanet.IdGara,
Categories.Filiere || ' ' || GaraRolskanet.Categorie,
Rolskanet.[Commune manifestation] as Place,
[Date de début],
[Date de fin],
CASE 
    WHEN categories.ID_Category in (21,23,25,27,29) AND Specialites.ID_Specialita in (10,11) then 2
    WHEN Categories.ID_Category<50 THEN Matrix.ID_Segment
    WHEN (Categories.ID_Category>=50 AND (Specialites.ID_Specialita=1 OR Specialites.ID_Specialita=2)) THEN 2
    WHEN (Categories.ID_Category>=50 AND (Specialites.ID_Specialita=5 OR Specialites.ID_Specialita=6)) THEN 4
    END as ID_Segment,
Specialites.ID_Specialita,
CASE 
    WHEN (Categories.ID_Category>=50 AND (Specialites.ID_Specialita=5 OR Specialites.ID_Specialita=6)) THEN 25
    ELSE Categories.ID_Category
END AS ID_Category,
count(DISTINCT Rolskanet.[Groupe]) as Participants,
Sexe,
'N' as Completed,
'0',
'3',
'1',
'6',
Competitions.ID_Competition      
From GaraRolskanet
INNER JOIN Specialites ON upper(GaraRolskanet.type)=upper(Specialites.Libellé)
INNER JOIN Categories ON upper(Categories.Categorie)=upper(GaraRolskanet.Categorie) and upper(Categories.Filiere)=upper(GaraRolskanet.Filiere)
INNER JOIN Matrix ON Matrix.ID_Specialita=Specialites.ID_Specialita AND Matrix.ID_Category=Categories.ID_Category
INNER JOIN Rolskanet ON upper(Rolskanet.Type)=upper(GaraRolskanet.Type) and upper(Rolskanet.Filière)=upper(GaraRolskanet.Filiere) AND upper(Rolskanet.[Épreuve])=upper(GaraRolskanet.Categorie)
INNER JOIN Competitions ON upper(Competitions.[Name])=upper(Rolskanet.Manifestation )
WHERE Specialites.ID_Specialita in (3,4,7,8,9)
GROUP BY Competitions.ID_Competition,GaraRolskanet.IdGara,ID_Segment;

DROP VIEW IF EXISTS BDD_MEDAILLE_DANSE;
CREATE VIEW BDD_MEDAILLE_DANSE AS
SELECT Athletes.Name, Athletes.Societa, Segments.Name as programme, Gara.element as verification, substr(Gara.Element, 1, length(Gara.element) -1) as element, 
case when substr(Gara.Element, length(Gara.element), 1) = 'B' then '0' else 
case when substr(Gara.Element, 1,  2) = 'NL' then '-1' ELSE
substr(Gara.Element, length(Gara.element), 1) end END  as niveau,
Elements.id_elementscat as cat_elem,
case when Athletes.Num_Licence IS NULL THEN 0
         else Athletes.Num_Licence
    END as num_licence,
GaraParams.ID_Competition as ID_Competition
FROM  Gara
INNER JOIN GaraParams ON (Gara.ID_GaraParams = GaraParams.ID_GaraParams and Gara.ID_Segment = GaraParams.ID_Segment)
INNER JOIN Participants ON (Participants.ID_GaraParams= Gara.ID_GaraParams AND Participants.ID_Segment=Gara.ID_Segment AND Participants.NumStartingList=Gara.NumPartecipante)
INNER JOIN Athletes ON (Athletes.ID_Atleta = Participants.ID_Atleta)
INNER JOIN GaraFinal ON (GaraFinal.ID_GaraParams=Gara.ID_GaraParams AND GaraFinal.ID_Segment = Gara.ID_Segment AND GaraFinal.NumPartecipante=Gara.NumPartecipante)
INNER JOIN Segments ON (Segments.ID_Segments=Gara.ID_Segment)
INNER JOIN Elements ON (Elements.Code=Gara.Element)
WHERE Segments.name not like '%Program%' 
and Gara.element <> 'ChStS';

DROP VIEW IF Exists BDD_medaille_free;
CREATE VIEW BDD_medaille_free as
SELECT Athletes.Name, Athletes.Societa, Gara.Element as element, Gara.bonus as bonus , count(Gara.element) as niveau,
case when substr(Gara.Element, 1, 2 ) = 'St' 
    THEN IIF (substr(Gara.Element, length(Gara.element), 1) = 'B', 0, substr(Gara.Element, length(Gara.element), 1))
    END as level,
    case when Athletes.Num_Licence IS NULL THEN 0
         else Athletes.Num_Licence
    END as num_licence,
    GaraParams.ID_Competition as ID_Competition
FROM  Gara
INNER JOIN GaraParams ON (Gara.ID_GaraParams = GaraParams.ID_GaraParams and Gara.ID_Segment = GaraParams.ID_Segment)
INNER JOIN Participants ON (Participants.ID_GaraParams= Gara.ID_GaraParams AND Participants.ID_Segment=Gara.ID_Segment AND Participants.NumStartingList=Gara.NumPartecipante)
INNER JOIN Athletes ON (Athletes.ID_Atleta = Participants.ID_Atleta)
INNER JOIN GaraFinal ON (GaraFinal.ID_GaraParams=Gara.ID_GaraParams AND GaraFinal.ID_Segment = Gara.ID_Segment AND GaraFinal.NumPartecipante=Gara.NumPartecipante)
INNER JOIN Segments ON (Segments.ID_Segments=Gara.ID_Segment)
INNER JOIN Elements ON (Elements.Code=Gara.Element)
WHERE
    ((Gara.pen not in ('<<<', '<<') 
    and Gara.bonus not like ('%*%') 
    and Gara.Element not in ('ChSt', 'FoSq', 'ClSq', 'Tr', 'ASq', 'comp', 'U', 'S', 'Br', 'Iv', 'CBD', 'CFD', 'HBD', 'HFD')
    and substr(Gara.Element, 1, 2 ) <> 'NL'
    and substr(Gara.Element, 1, 2 ) <> 'St'  
    and Segments.id_segments in (1, 2))
OR    
    (Gara.bonus not like ('%*%') 
    and Gara.Element in ('U', 'S', 'Br', 'Iv', 'CBD', 'CFD', 'HBD', 'HFD')
    and substr(Gara.Element, 1, 2 ) <> 'NL'
    and substr(Gara.Element, 1, 2 ) <> 'St'  and Segments.id_segments in (1, 2))
OR
    (Gara.pen in ('<<') 
    and Gara.bonus not like ('%*%') 
    and Gara.element = '2A')
OR
    (substr(Gara.Element, 1, 2 ) = 'St' 
    or substr(Gara.Element, 1, 4 ) = 'NLSt'));

INSERT INTO PanelJudge(ID_Judge,ID_GaraParams,Role)
SELECT PanelExemple.ID_Judge,GaraParams.ID_GaraParams,PanelExemple.Role
FROM GaraParams, PanelExemple;

INSERT INTO Participants(ID_GaraParams, ID_Segment, ID_Atleta)
SELECT 
    GaraParams.ID_GaraParams,
    GaraParams.ID_Segment,
    Athletes.ID_Atleta
FROM Rolskanet
INNER JOIN Specialites ON UPPER(Specialites.Libellé)=UPPER(Rolskanet.Type)
INNER JOIN Athletes ON Athletes.Num_Licence=Rolskanet.[Numéro de licence] AND Athletes.ID_Specialita=Specialites.ID_Specialita
INNER JOIN GaraParams ON upper(GaraParams.Name)=UPPER((Rolskanet.Filière) || ' ' || Rolskanet.[Sportif catégorie âge]) AND GaraParams.ID_Specialita=Specialites.ID_Specialita
WHERE Athletes.ID_Specialita in (1,2,5,6,10,11) 
GROUP BY  Athletes.ID_Atleta, GaraParams.ID_Category, GaraParams.ID_Specialita, GaraParams.ID_Segment;

WITH WindowOrder AS (
    SELECT ID_Atleta,
           ID_Garaparams,
           ID_Segment,
           row_number() OVER (PARTITION BY ID_GaraParams,
           ID_Segment ORDER BY ID_Atleta) AS RowNumber
      FROM Participants
)
UPDATE Participants
   SET NumStartingList = RowNumber
  FROM WindowOrder
 WHERE Participants.ID_GaraParams = WindowOrder.ID_GaraParams AND 
       Participants.ID_Segment = WindowOrder.ID_Segment AND 
       Participants.ID_Atleta = WindowOrder.ID_Atleta;
       

INSERT INTO Participants(ID_GaraParams, ID_Segment, ID_Atleta,InfoFIeld5)
SELECT 
    GaraParams.ID_GaraParams,
    GaraParams.ID_Segment,
    Athletes.ID_Atleta,
    Rolskanet.Commentaire
FROM Rolskanet
INNER JOIN Specialites ON UPPER(Specialites.Libellé)=UPPER(Rolskanet.Type)
INNER JOIN Athletes ON Athletes.name=Rolskanet.Groupe AND Athletes.Num_Licence=Rolskanet.[Épreuve] AND Athletes.ID_Specialita=Specialites.ID_Specialita 
INNER JOIN GaraParams ON upper(GaraParams.Name)=UPPER((Rolskanet.Filière) || ' ' || Rolskanet.[Épreuve]) AND GaraParams.ID_Specialita=Specialites.ID_Specialita
WHERE Athletes.ID_Specialita in (3,4,7,8,9)
GROUP BY  Athletes.ID_Atleta, GaraParams.ID_Category, GaraParams.ID_Specialita, GaraParams.ID_Segment;

WITH WindowOrder AS (
    SELECT ID_Atleta,
           ID_Garaparams,
           ID_Segment,
           row_number() OVER (PARTITION BY ID_GaraParams,
           ID_Segment ORDER BY ID_Atleta) AS RowNumber
      FROM Participants
)
UPDATE Participants
   SET NumStartingList = RowNumber
  FROM WindowOrder
 WHERE Participants.ID_GaraParams = WindowOrder.ID_GaraParams AND 
       Participants.ID_Segment = WindowOrder.ID_Segment AND 
       Participants.ID_Atleta = WindowOrder.ID_Atleta;

/*
DROP VIEW IF EXISTS sequence_danse;create view sequence_danse as    SELECT num_licence, name, societa, 'sequence', niveau    FROM BDD_MEDAILLE_DANSE where cat_elem in ('10', '12')    order by num_licence, name, niveau desc;DROP VIEW IF EXISTS total_score;create view total_score As   SELECT num_licence, name,  societa, 'Di' as element,  round(avg(niveau),0) as niveau    FROM BDD_MEDAILLE_DANSE where cat_elem in ('13')    group by num_licence, name, societa     UNION    SELECT num_licence, name, societa, 'Seq'as element,         sum(niveau) as niveau from (SELECT num_licence, name, niveau, ROW_NUMBER() OVER(PARTITION BY num_licence, name) AS row_number,         societa     FROM sequence_danse     order by name, niveau desc) As A    where row_number <= 2     group by num_licence, name, societa       UNION    SELECT num_licence, name, societa, 'Tr'as element, max(niveau) as niveau     FROM BDD_MEDAILLE_DANSE     where  programme in ('Free Dance', 'Style Dance') and element in ('Tr', 'NLT')     group by num_licence, name, societa;DROP VIEW IF EXISTS medailles_danse;
create view medailles_danse as With RECURSIVE ext(num_licence, name, societa,  danse_imposée, travelling, sequence_pas, total) AS (select  num_licence, name, societa,    sum(niveau) filter (where element = 'Di') ,  sum(niveau) filter (where element = 'Tr') ,  sum(niveau) filter (where element = 'Seq') ,  (     (sum(niveau) filter (where element = 'Di')) +      (sum(niveau) filter (where element = 'Tr')) +     (sum(niveau) filter (where element = 'Seq'))   ) from total_score group by num_licence, name, societa )select num_licence, name, societa, danse_imposée, travelling, sequence_pas, total,  IIF( total = 0 , 'Médaille Préliminaire',       IIF( total = 1 , 'Médaille Bronze',            IIF( total between 2 and 3 , 'Médaille Argent',                 IIF( total = 4 , 'Médaille Vermeil',                      IIF( total = 5 , 'Médaille Or',                          IIF( total >= 6 , 'Médaille Platine', '--------'                            )                        )                   )              )         )     ) as médaille  from ext  Order by médaille,name;


DROP VIEW IF EXISTS Ext_Medaille_free;
CREATE VIEW Ext_Medaille_free as
select
num_licence, name, societa,  
count(distinct(element)) filter (where element = '1W') as W1,
count(distinct(element)) filter (where element = '1A') as A1,
count(distinct(element)) filter (where element = '2A') as A2,
count(distinct(element)) filter (where element = '3A') as A3,  
count(distinct(element)) filter (where substr(element, 1, 1) = '1' and element <> '1W'  ) as Ju1,
count(distinct(element)) filter (where substr(element, 1, 1) = '2') as Ju2,
count(distinct(element)) filter (where substr(element, 1, 1) = '3') as Ju3,
count(element) filter (where element = 'U') as U,
count(element) filter (where element = 'U' and bonus = '%+') as UB,
count(element) filter (where element = 'S') as S,
count(element) filter (where element like 'C%D%') As C, 
count(element) filter (where element like 'H%D%') As He, 
count(element) filter (where element = 'Br') As Br, 
count(element) filter (where element = 'In') as Iv,
max(level) filter (where substr(element, 1, 2) = 'St') as FoSq
from BDD_medaille_free
group by num_licence,name, societa;

DROP VIEW IF EXISTS Base_medaille_free;
create view Base_medaille_free as
SELECT num_licence, name, Societa,
    W1 as valse,
   (A1) as Axel,
   (A2+A3) as DoubleAxel,
   (Ju1) as Simple,
   (Ju2+Ju3) as Jump,
   U as Up,
   S as Sit,
   C as Camel,
   (U+S+C+He+Iv+Br+UB) as Spin,
   FoSq as Step,
   CASE
        WHEN (A1+A2+A3)>=1
             AND (Ju2+Ju3)>=2
             AND (S>=1 AND (S+C+He+Iv+Br)>=3)
             AND FoSq>=1
       THEN 'Médaille Platine'
       WHEN (A1+A2+A3)>=1
             AND (Ju2+Ju3)>=2
             AND (S>=1 AND C>=1)
             AND FoSq>=0
       THEN 'Médaille Or'
       WHEN (A1+A2+A3)>=1
             AND (Ju2+Ju3)>=1
             AND (S>=1 OR C>=1)
             AND FoSq>=0
       THEN 'Médaille Vermeil'
       WHEN (A1+A2+A3)>=1
             AND (S>=1 OR C>=1)
             AND FoSq>=0
       THEN 'Médaille Argent'
       WHEN (Ju1+Ju2+Ju3)>=4
             AND (U+S+C+He+Iv+Br+UB)>=1
             AND FoSq>=0
       THEN 'Médaille Bronze'
       WHEN (Ju1+Ju2+Ju3)>=2
             AND (U+S+C+He+Iv+Br+UB)>=1
             AND FoSq>=0
       THEN 'Médaille Préliminaire'
       ELSE '------'
   END as médaille                             
FROM Ext_Medaille_free
Order by médaille,name;




*/