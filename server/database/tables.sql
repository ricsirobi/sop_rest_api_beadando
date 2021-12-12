DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS lego;
DROP TABLE IF EXISTS user_lego;
DROP TABLE IF EXISTS category;


CREATE TABLE users (
id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
username VARCHAR(60) NOT NULL,
password VARCHAR(500) NOT NULL,
isAdmin BIT NOT NULL DEFAULT 0);

CREATE TABLE lego (
id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
code VARCHAR(10) NOT NULL UNIQUE,
name VARCHAR(255) NOT NULL,
category INT NOT NULL DEFAULT 12,
HUFprice INT NOT NULL);

CREATE TABLE user_lego (
id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
uid INT NOT NULL,
lid INT NOT NULL);

CREATE TABLE category (
id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
name VARCHAR(60) NOT NULL);

ALTER TABLE lego ADD CONSTRAINT lego_category_category_id FOREIGN KEY (category) REFERENCES category(id);
ALTER TABLE user_lego ADD CONSTRAINT user_lego_uid_users_id FOREIGN KEY (uid) REFERENCES users(id);
ALTER TABLE user_lego ADD CONSTRAINT user_lego_lid_lego_id FOREIGN KEY (lid) REFERENCES lego(id);


INSERT INTO category(name) VALUES ("Star Wars");
INSERT INTO category(name) VALUES ("City");
INSERT INTO category(name) VALUES ("Ninjago");
INSERT INTO category(name) VALUES ("Super Heroes");
INSERT INTO category(name) VALUES ("Sports");
INSERT INTO category(name) VALUES ("Minecraft");
INSERT INTO category(name) VALUES ("Technic");
INSERT INTO category(name) VALUES ("Super Mario");
INSERT INTO category(name) VALUES ("Minifigures");
INSERT INTO category(name) VALUES ("Ünnep");
INSERT INTO category(name) VALUES ("Architecture");
INSERT INTO category(name) VALUES ("Other");

INSERT INTO lego( code, name, category, HUFprice) VALUES ("10236","Ewok falu",1,199990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("30228","Rendőrségi ATV",2,1990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("70606","Spinjitzu kiképzés",3,5990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("10937","Szökés az Arkham elmegyógyintézetből",4,99990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("30203","Mini Golf",5,1290);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("853610","Skin Pack 2",6,13990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("42021","Motoros szán",7,14990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("71360","Mario kalandjai kezdőpálya",8,16990);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("71010","Szörnyek meglepetéscsomag 14. széria",9,1590);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("30478","A Mikulás puttonya",10,2490);
INSERT INTO lego( code, name, category, HUFprice) VALUES ("21006","A Fehér Ház",11,49990);
INSERT INTO lego( code, name, HUFprice) VALUES ("9999","Valami más",10990);

INSERT INTO users(username, password, isAdmin) VALUES ("RicsiRobi","RicsiRobi",0);
INSERT INTO users(username, password, isAdmin) VALUES ("RicsiRobiAdmin","RicsiRobiAdmin",1);


DROP TABLE IF EXISTS tok;


CREATE TABLE tok (
id INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
value VARCHAR(500) NOT NULL);

