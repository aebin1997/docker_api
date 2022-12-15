create table tb_user_best_record
(
    user_id         int unsigned     not null
        primary key,
    score           tinyint unsigned null,
    score_updated   bigint unsigned  null,
    longest         decimal(5, 2)    null,
    longest_updated bigint unsigned  null
);