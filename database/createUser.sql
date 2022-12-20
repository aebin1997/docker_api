# use mysql;
# -- select host, user, password from user;
# select host, user, authentication_string from user;
# 
# create user 'aebin'@'%' identified by 'mysql1q2w#E4R';
# flush privileges;
    
create database ParkAebinDB;

DROP USER 'aebin'@'%';

create user 'aebin'@'%' identified by 'mysql1q2w#E4R';
grant all privileges on ParkAebinDB.* to 'aebin'@'%';

show grants for 'aebin'@'%';

FLUSH PRIVILEGES;