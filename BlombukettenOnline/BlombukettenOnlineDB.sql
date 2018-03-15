CREATE TABLE Category
(
Id int Primary Key Identity  (1,1),
Name nvarchar(MAX) not null,
Price decimal not null,
);

CREATE TABLE Product
(
  Id int Primary Key Identity  (1,1),
  Name nvarchar(MAX) not null,
  [Description] nvarchar(MAX) not null, 
  ColorId int FOREIGN KEY REFERENCES Color(Id),
);

CREATE TABLE [Address]
(
Id int Primary Key Identity  (1,1),
Street nvarchar(MAX) not null,
City nvarchar(MAX) not null,
PostalCode nvarchar(MAX) not null,
--StreetNumber nvarchar(MAX) not null,
);

CREATE TABLE Email
(
Id int Primary Key Identity  (1,1),
Email nvarchar(MAX) not null,
Subscribe bit,
);

CREATE TABLE [Order]
(
Id int Primary Key Identity (1,1),
AddressId int FOREIGN KEY REFERENCES [Address](Id),
TotalPrice decimal,
Comment nvarchar(MAX),
EmailId int FOREIGN KEY REFERENCES Email(Id),
--Phone nvarchar(MAX) not null,
--Delivered bit,
);

CREATE TABLE Color
(
Id int Primary Key Identity  (1,1),
Name nvarchar(MAX) not null,
);

CREATE TABLE Boquett
(
Id int Primary Key Identity (1,1),
Suma decimal not null,
OrderId int FOREIGN KEY REFERENCES [Order](Id),
);

CREATE TABLE ProductCategory
(
ProductId INT REFERENCES Product(Id),
CategoryId INT REFERENCES Category(Id),
PRIMARY KEY(ProductId, CategoryId)
);

CREATE TABLE ProductBoquett
(
ProductId INT REFERENCES Product(Id),
BoquettId INT REFERENCES Boquett(Id),
PRIMARY KEY(ProductId, BoquettId)
);
