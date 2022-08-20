﻿CREATE TABLE USER_REGISTRATION (USERID INT IDENTITY(100,1) PRIMARY KEY,NAME VARCHAR(20) NOT NULL,EMAIL VARCHAR(50) UNIQUE NOT NULL,MOBILE VARCHAR(10) UNIQUE NOT NULL,
PASSWORD VARCHAR(16) NOT NULL,ADDRESS VARCHAR(20) NOT NULL);

SELECT * FROM USER_REGISTRATION

CREATE TABLE FOOD_ITEM (FOODID INT IDENTITY(100,1) PRIMARY KEY,NAME VARCHAR NOT NULL);

CREATE TABLE FOOD_TYPE (TYPEID INT IDENTITY(1,1) PRIMARY KEY,FOODID INT, NAME VARCHAR NOT NULL, PRICE INT NOT NULL, QUANTITY INT NOT NULL,
FOREIGN KEY (FOODID) REFERENCES FOOD_ITEM(FOODID));

CREATE TABLE ADDTOCART (ID INT PRIMARY KEY IDENTITY(1,1),USERID INT,TYPEID INT,NAME VARCHAR,QUANTITY INT , PRICE INT

FOREIGN KEY (USERID) REFERENCES USER_REGISTRATION(USERID),
FOREIGN KEY (TYPEID) REFERENCES FOOD_TYPE(TYPEID)

);
INSERT INTO FOOD_ITEM (NAME) VALUES('KOTHTHU');
INSERT INTO FOOD_ITEM (NAME) VALUES('BAKERY');
INSERT INTO FOOD_ITEM (NAME) VALUES('PIZZA');
INSERT INTO FOOD_ITEM (NAME) VALUES('RICE');
INSERT INTO FOOD_ITEM (NAME) VALUES('DESERT');

INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (105,'CHICKEN KOTHTHU',400,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (105,'EGG KOTHTHU',350,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (105,'CHEESE KOTHTHU',400,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (106,'SUBMARINE',400,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (106,'BURGER',400,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (106,'BREAD',200,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (106,'CAKE',350,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (107,'CHICKEN PIZZA',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (107,'CHEESE PIZZA',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (107,'CRISPY PIZZA',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (108,'BIRIYANI',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (108,'MILK RICE',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (108,'YELLOW RICE',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (108,'FRIED RICE',850,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (109,'ICE CREAM',200,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (109,'JELLY',200,10);
INSERT INTO FOOD_TYPE(FOODID,NAME,PRICE,QUANTITY) VALUES (109,'WATALAPPAN',500,10);