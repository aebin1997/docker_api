create table tb_user (
    idx int unsigned primary key auto_increment, 
    user_id varchar(255) not null, user_pw varchar(255) not null,
    life_best_score int null, 
    created timestamp not null, 
    updated timestamp default current_timestamp,
    deleted bool default 0
    );