CREATE TABLE Category
(
Id int Primary Key Identity(1,1),
Name nvarchar(MAX) not null,
ParentId int FOREIGN KEY REFERENCES Category(Id),
Active bit,
);

INSERT INTO Category VALUES
('Ros',null,'1'),
('Lång Ros','1', '1'),
('Dahlia',null, '1'),
('Stor Dahlia','3', '1'),
('Lilja',null, '1'),
('Liten Lilja','5', '1')

CREATE TABLE Color
(
Id int Primary Key Identity  (1,1),
Name nvarchar(MAX) not null,
Active bit,
);

INSERT INTO Color VALUES
('Röd','1'),
('Blå','1'),
('Lila','1'),
('Gul','1'),
('Rosa','1')

CREATE TABLE Product
(
  Id int Primary Key Identity  (1,1),
  Name nvarchar(MAX) not null,
  [Description] nvarchar(MAX),
  Price decimal not null,
  ColorId int FOREIGN KEY REFERENCES Color(Id) not null,
  CategoryId int FOREIGN KEY REFERENCES Category(Id) not null,
  Active bit not null,
  Picture nvarchar(max) not null,
);

INSERT INTO Product VALUES
('Ros Röd','..','35','1', '1','1','BildSaknas.jpg'),
('Ros Lila','..','35','3', '2','1','BildSaknas.jpg'),
('Dahlia Blå','..','45','2','4','1','BildSaknas.jpg'),
('Lilja Röd','..','25','1','5','1','BildSaknas.jpg')

CREATE TABLE [Address]
(
Id int Primary Key Identity  (1,1),
Street nvarchar(MAX) not null,
PostalCode nvarchar(MAX) not null,
);

CREATE TABLE Email
(
Id int Primary Key Identity  (1,1),
Email nvarchar(MAX) not null,
Subscribe bit not null,
);

CREATE TABLE Store
(
	Id int primary key identity(1,1),
	Name nvarchar(MAX) not null,
);

INSERT INTO Store VALUES
('Göteborg'),
('Stockholm'),
('Malmö')

CREATE TABLE PaymentMethod
(
	Id	int primary key identity(1,1),
	Method nvarchar(MAX) not null,
);

INSERT INTO PaymentMethod VALUES
('Betala i butik'),
('Internetbank'),
('Paypal')

CREATE TABLE DeliveryMethod
(
	Id int primary key identity(1,1),
	Method nvarchar(max) not null,
);

INSERT INTO DeliveryMethod VALUES
('Hämta i butik'),
('Hemleverans'),
('leverans')


CREATE TABLE [Order]
(
Id int Primary Key Identity (1,1),
AddressId int FOREIGN KEY REFERENCES [Address](Id),
TotalPrice decimal not null,
Comment nvarchar(MAX),
EmailId int FOREIGN KEY REFERENCES Email(Id) not null,
IsDelivered bit not null,
PaymentId int FOREIGN KEY REFERENCES PaymentMethod(Id) not null,
DeliveryId int FOREIGN KEY REFERENCES DeliveryMethod(Id) not null,
StoreId int FOREIGN KEY REFERENCES Store(Id) not null
);

CREATE TABLE Boquett
(
Id int Primary Key Identity (1,1),
"Sum" decimal not null,
Quantity int not null,
OrderId int FOREIGN KEY REFERENCES [Order](Id) not null,
);

CREATE TABLE ProductBoquett
(
ProductId INT REFERENCES Product(Id) not null,
BoquettId INT REFERENCES Boquett(Id) not null,
Quantity INT not null,
PRIMARY KEY(ProductId, BoquettId)
);


