create table tb_user_info_by_club
(
    user_id  int unsigned    not null,
    club     varchar(10)     not null,
    distance decimal(5, 2)   null,
    updated  bigint unsigned null,
    primary key (user_id, club)
);