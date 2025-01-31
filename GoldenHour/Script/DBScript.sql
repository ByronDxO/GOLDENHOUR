create table GT_User(
usu_idUser int primary key identity ,
usu_firstname varchar(250) ,
usu_lastName varchar(250),
usu_password varchar(500),
usu_username varchar(50),
usu_dateIni date ,
usu_status int  
);

create table GT_Module(
mod_idModule int primary key identity,
mod_name varchar(100),
mod_status int ,
mod_date date 
);



create table GT_AssignamentUserModule(
aum_idAssignament int primary key identity ,
fk_idUser int ,
fk_idModule int ,
aum_status int ,
aum_date date ,

foreign key (fk_idUser) references GT_User(fk_id)

)