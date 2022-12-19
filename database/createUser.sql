use mysql;
-- select host, user, password from user;
select host, user, authentication_string from user;

create user 'aebin'@'%' identified by 'mysql1q2w#E4R';
flush privileges;
    
create database ParkAebinDB;

-- grant select, insert, update on DBname.* to 'userID'@'%';
GRANT SELECT, INSERT, UPDATE, DELETE, CREATE, DROP, RELOAD, SHUTDOWN, PROCESS, FILE, REFERENCES, INDEX, ALTER, SHOW DATABASES, SUPER, CREATE TEMPORARY TABLES, LOCK TABLES, EXECUTE, REPLICATION SLAVE, REPLICATION CLIENT, CREATE VIEW, SHOW VIEW, CREATE ROUTINE, ALTER ROUTINE, CREATE USER, EVENT, TRIGGER, CREATE TABLESPACE, CREATE ROLE, DROP ROLE ON `parkaebindb`.* TO `aebin`@`localhost` WITH GRANT OPTION

Grants for aebin@localhost: GRANT SELECT, INSERT, UPDATE, DELETE ON `parkaebindb`.* TO `aebin`@`localhost`