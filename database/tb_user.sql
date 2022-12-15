# create table tb_user (
#     idx int unsigned primary key auto_increment, 
#     user_id varchar(255) not null, user_pw varchar(255) not null,
#     life_best_score int null, 
#     created timestamp not null, 
#     updated timestamp default current_timestamp,
#     deleted bool default 0
#     );

create table tb_user
(
    id       int unsigned auto_increment
        primary key,
    username varchar(30)          null,
    password varchar(50)          null,
    name     varchar(30)          not null,
    created  bigint unsigned      not null,
    updated  bigint unsigned      null,
    deleted  tinyint(1) default 0 null
);