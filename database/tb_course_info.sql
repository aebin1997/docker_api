
DROP TABLE IF EXISTS `ParkAebinDB`.`tb_course_info`;
create table `ParkAebinDB`.tb_course_info
(
    id          int unsigned auto_increment
        primary key,
    course_name varchar(60) null
);