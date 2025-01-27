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

foreign key (fk_idUser) references GT_User(usu_idUser),
foreign key (fk_idModule) references GT_Module(mod_idModule)

)


create table GT_MeanPayment(
mep_idMeanPayment int identity primary key ,
mep_name varchar(50),
mep_status int ,
mep_date date 

)

create table GT_Payment(
pay_idPayment int identity primary key ,
pay_transaction varchar(100) ,
pay_date date ,
fk_idMeanPayment int ,
foreign key (fk_idMeanPayment) references GT_MeanPayment(mep_idMeanPayment)

)

create table GT_Modifier(
mdf_idModifier int identity primary key ,
mdf_name varchar(250),
mdf_description varchar(500),
mdf_status int ,
mdf_percentage int 

)

create table GT_Category(
cat_idCategory int identity primary key ,
cat_name varchar(100),
cat_description varchar(250),
cat_status int ,
cat_date date 

)


create table GT_Product (
pro_idProduct int identity primary key ,
pro_name varchar(250),
pro_description varchar(500),
pro_status int ,
pro_date date ,
pro_total SMALLMONEY,
pro_stock int ,
pro_pathImage varchar(500),
fk_category int ,
foreign key (fk_category) references GT_Category(cat_idCategory)
)


create table GT_Receipt(
rec_idReceipt int identity primary key ,
rec_total SMALLMONEY,
rec_client varchar(250),
rec_mail varchar(250),
rec_date date ,
rec_status int ,
fk_idUser int ,
fk_idPayment int ,
fk_idModifier int ,
foreign key (fk_idUser) references GT_User(usu_idUser),
foreign key (fk_idPayment)references GT_Payment (pay_idPayment),
foreign key (fk_idModifier) references GT_Modifier(mdf_idModifier)
)

create table GT_DetailReceipt(
dre_idDetailReceipt int identity primary key ,
dre_date date ,
dre_stock int ,
dre_total smallmoney ,
fk_idProduct int ,
fk_idReceipt int ,
foreign key (fk_idProduct) references GT_Product(pro_idProduct),
foreign key (fk_idReceipt) references GT_Receipt(rec_idReceipt)
)