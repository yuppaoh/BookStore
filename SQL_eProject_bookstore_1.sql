CREATE DATABASE BookStore
ON PRIMARY
	(NAME = N'StdCmd', 
	FILENAME = N'E:\sql_eProject\BookStore.mdf', 
	SIZE = 10MB, MAXSIZE = UNLIMITED, FILEGROWTH = 1MB)
LOG ON
	(NAME = N'StdCmd_Log',
	FILENAME = N'E:\sql_eProject\BookStore.ldf',
	SIZE = 3MB, MAXSIZE = 2048GB, FILEGROWTH = 10%)


use BookStore


/*==============================================================*/
/* Table: USERS                                                 */
/*==============================================================*/

create table Users 
(
   UserId             integer                        not null,
   Email              varchar(25)                    null,
   UserName           varchar(50)                    not null,
   UserPassword       varchar(25)                    null,
   PathImage          varchar(225)                   null,
   PhoneNo            varchar(18)                    null,
   UserRole           varchar(10)                    null,
   primary key (UserId)
);

/*==============================================================*/
/* Table: CATEGORIES, MANUFACTUREs, PRODUCTS                       */
/*==============================================================*/
create table Categories 
(
   CategoryId     varchar(2)     not null,
	CategoryName   nvarchar(256)  not null,
	Primary key (CategoryId)
);

create table Manufactures 
(
   ManufactureId       varchar(3)                  not null,
   ManufactureName     varchar(150)                null,
   ManufactureAddress  varchar(150)                 null,
   PhoneNo             varchar(18)                 null,
   Note                varchar(256)                null,
   primary key (ManufactureId)
);

create table Products 
(
   ProductId         varchar(7)                    not null,
   CategoryId        varchar(2)                    not null,
   ProductImage      varchar(80)                   null,
   ProductName       varchar(150)                  null,
   ProductType       varchar(25)                   null,
   ManufactureId     varchar(3)                    null,
   Author            varchar(25)                   null,
   ReleaseDate       date                          null,
   UnitPrice         float                         null,
   Quantity          integer                       null,
   Note              varchar(256)                  null,
   primary key (ProductId)
);

/*==============================================================*/
/* Table: CUSTOMERS, PAYMENTS, ORDERS                                                */
/*==============================================================*/
create table Customers 
(
   CustomerId           varchar(10)      not null,
   CustomerName         varchar(50)      null,
   CustomerAddress      varchar(100)     null,
   PhoneNo              varchar(18)      null,
   CustomerPassword     varchar(20)      null,
   CustomerStatus       varchar(10)      not null    default 'Enable',
   primary key (CustomerId)
);

create table Payments 
(
   PaymentId               integer            not null,
   PaymentName             varchar(50)        null,
   primary key (PaymentId)
);

create table Orders 
(
   OrderId            varchar(8)              not null,
   CustomerId         varchar(10)             not null,
   OrderDate          date                    null,
   PlaceOfDelivery    varchar(150)            null,
   Distance           integer                 null,
   PaymentId          integer                 null,    
   DeliveryDate       date                    null,
   OrderStatus        varchar(25)             null,
   primary key (OrderId)
);

/*==============================================================*/
/* Table: ORDERDETAIL                                           */
/*==============================================================*/
create table OrderDetails 
(
   OrderId            varchar(8)                     not null,
   ProductId          varchar(7)                     not null,
   Quantity           integer                        null,
   UnitPrice          float                          null,
   constraint PK_ORDERDETAIL primary key clustered (OrderId, ProductId)
);



alter table Products add constraint product_categories foreign key (CategoryId) references Categories(CategoryId);
alter table Products add constraint product_manufacture foreign key (ManufactureId) references Manufactures(ManufactureId);

alter table Orders add constraint order_customers foreign key (CustomerId) references Customers(CustomerId);
alter table Orders add constraint order_payments foreign key (PaymentId) references Payments(PaymentId);

alter table OrderDetails add constraint orderdetail_orders foreign key (OrderId) references Orders(OrderId);
alter table OrderDetails add constraint orderdetail_products foreign key (ProductId) references Products(ProductId);

