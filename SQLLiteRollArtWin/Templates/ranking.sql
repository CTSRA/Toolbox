SELECT
    RANK() OVER (
        PARTITION BY e.name, a.name 
        ORDER BY SUM(b.total) DESC
    ) as "Ranking",
    f.name as "Patineur(s)",
    a.name as "Catégorie",
    e.name as "Discipline",
    c.name as "Catégorie_WS",
    f.num_licence as "Num_Licence",
    f.societa as "Club",
    SUM(b.total) as "Score"

FROM GaraParams a
         JOIN Garafinal b
              ON a.id_garaparams = b.id_garaparams
                  AND a.id_segment = b.id_segment
         JOIN Category c
              ON a.id_category = c.id_category
         JOIN specialita e
              ON e.id_specialita = a.id_specialita
         JOIN Athletes f
              ON f.id_atleta = b.id_atleta
GROUP BY
    f.name,
    a.name,
    e.name,
    c.name,
    f.num_licence,
    f.societa
ORDER BY
    e.name,
    a.name,
    "Score" DESC;