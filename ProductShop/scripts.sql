CREATE DATABASE ProductShop;

USE ProductShop;

CREATE TABLE Category(
                         Id INT IDENTITY(1,1) PRIMARY KEY,
                         Name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Product(
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL UNIQUE,
                        CategoryId INT NOT NULL FOREIGN KEY REFERENCES Category(Id)
);

DROP TABLE Category;
DROP TABLE Product;

INSERT INTO Category(Name) VALUES('Eletronics');
INSERT INTO Category(Name) VALUES('Smartphone');

SELECT * FROM Category;

INSERT INTO Product(Name, CategoryId) VALUES('iPhone 15', 2);
INSERT INTO Product(Name, CategoryId) VALUES('TV Samsung 50', 1);

SELECT * FROM Product;
