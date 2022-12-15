
DROP TABLE IF EXISTS `ParkAebinDB`.`tb_user_info_by_course`;
create table `ParkAebinDB`.tb_user_info_by_course
(
    id        int unsigned auto_increment
        primary key,
    user_id   int unsigned     not null,
    course_id int unsigned     not null,
    score     tinyint unsigned null,
    longest   decimal(5, 2)    null,
    updated   bigint unsigned  not null
);

create index course_id
    on `ParkAebinDB`.tb_user_info_by_course (course_id);