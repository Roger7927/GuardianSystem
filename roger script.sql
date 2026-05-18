USE hotel_db;

-- Consulta: Descrição, Data e Custo das Manutenções
SELECT
    M.descricao,
    M.data_manutencao,
    M.custo,
    Q.id_quarto,
    Q.numero_de_leitos
FROM
    Manutencao M
JOIN
    Quarto Q ON M.id_quarto = Q.id_quarto;