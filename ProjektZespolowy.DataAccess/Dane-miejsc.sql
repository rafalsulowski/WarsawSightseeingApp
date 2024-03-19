USE ProjektZespolowy
--use master
DELETE FROM PlacesTrips
DELETE FROM placeAvailabilityTimes
DELETE FROM Places
DELETE FROM placeAvailabilityTimes

SET IDENTITY_INSERT Places ON

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (1,'Muzeum Narodowe',	'52.2317404268188,21.024337782647017',	'Muzeum sztuki i rze�by zwi�zanej z histori� staro�ytna i wsp�czesn� Polski',	25,	120,	'al. Jerozolimskie 3, 00-495 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY()  ,0,600,600,600,600,600,600,0,1080,1080,1080,1200,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (2,'Muzeum Powstania Warszawskiego',	'52.23277747505596,20.980874792519103',	'Muzeum z wystawami przedstawiaj�cymi przebieg powstania warszawskiego i �ycie os�b w nim uczestnicz�cym',	30,	120,	'Grzybowska 79, 00-844 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),480,0,480,480,480,600,600,1080,0,1080,1080,1080,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (3,'Zamek Kr�lewski',	'52.247752464638275,21.014605598114446',	'XIV-wieczny pa�ac oferuj�cy zwiedzanie sal oraz galerii sztuki',	23,	90,	'plac Zamkowy 4, 00-277 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,600,600,600,600,600,600,0,1020,1020,1020,1020,1020,1020)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (4,'Pa�ac Kultury i Nauki',	'52.23194394437168,21.005959090463246',	'Pa�ac wybudowany w po�owie XX wieku, w kt�rym odbywa si� wiele wydarze� kulturowych. Posiada taras widokowy na 30 pi�trze.',	25,	60,	'plac Defilad 1, 00-901 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,600,1200,1200,1200,1200,1200,1200,1200)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (5,'Centrum Nauki Kopernik',	'52.242005001296334,21.028787109382893',	'Nowoczesne centrum naukowe z interaktywnymi wystawami i labolatoriami',	40,	180,	'Wybrze�e Ko�ciuszkowskie 20, 00-390 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),540,540,540,540,540,540,540,1080,1080,1080,1080,1200,1140,1140)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (6,'Muzeum Fryderyka Chopina',	'52.2365339111456,21.022931471860616',	'Muzeum skupiaj�ce si� na historii i tw�rczo�ci Fryderyka Chopina',	25,	60,	'Pa�ac Gni�skich, Ok�lnik 1, 00-368 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,600,600,600,600,600,600,0,1080,1080,1080,1080,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (7,'PGE Narodowy',	'52.23942018809969,21.045250325519294',	'Stadion pi�ki no�nej, kt�ry jest miejscem wielu wydarze� sportowych i wystaw. Umo�liwia te� zwiedzanie z przewodnikiem',	25,	60,	'al. Ksi�cia J�zefa Poniatowskiego 1, 03-901 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),660,660,660,660,660,660,660,1080,1080,1080,1080,1080,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (8,'�azienki Kr�lewskie',	'52.21540461751925,21.03496890964672',	'Ogromny park niedaleko centrum miasta',	0,	60,	'Ujazd�w, 00-460 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (9,'Ogrody Pa�acu Jana III w Wilanowie',	'52.16537247908525,21.09037854559097',	'Ogrody pe�ne r�norodnych kwiat�w, drzew i krzew�w ozdabiaj�ce pa�ac kr�lewski.',	10,	90,	'Stanis�awa Kostki Potockiego 10/16, 02-958 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (10,'Muzeum Pa�acu Jana III w Wilanowie',	'52.16537247908525,21.09037854559097',	'XVII-wieczny pa�ac zawieraj�cy odrestaurowane sale, wystawy i galerie sztuki',	35,	180,	'Stanis�awa Kostki Potockiego 10/16, 02-958 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,600,960,960,960,960,960,960,960)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (11,'Z�ote Tarasy',	'52.23017829799075,21.002781792758437',	'Galeria handlowa mieszcz�ce si� w samym centrum miasta. Zawiera m.in. sklepy odzie�owe, restauracje, skelpy spo�ywcze.',	0,	60,	'Z�ota 59, 00-120 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),540,540,540,540,540,540,0,1320,1320,1320,1320,1320,1320,0)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (12,'Westfield Arkadia',	'52.25734712517806,20.984648511452917',	'Jedna z najwi�kszych galerii handlowych w mie�cie',	0,	60,	'al. Jana Paw�a II 82, 00-175 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,0,1320,1320,1320,1320,1320,1320,0)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (13,'Westfield Mokot�w',	'52.18061362356889,21.005435017212694',	'Najwi�ksza galeria handlowe w po�udniowej cz�ci Warszawy',	0,	60,	'Wo�oska 12, 02-675 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,0,1320,1320,1320,1320,1320,1320,0)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (14,'Park Szcz�liwicki',	'52.20492644301373,20.95955463061865',	'Du�y park, w kt�rym mo�na odpocz�� nad jeziorem lub zjecha� na nartach ze sztucznego stoku',	0,	90,	'02-384 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (15,'Park Edwarda Szyma�skiego',	'52.2340116991027,20.951806671971315',	'Nowoczesny park z wieloma �cie�kami dla pieszych, rower�w oraz deskorolek',	0,	60,	'Elekcyjna, Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (16,'Park Moczyd�o',	'52.24213165848122,20.95169501148924',	'Park zawieraj�cy stawy, bazar oraz g�rk� z widokiem na panoram� centrum miasta',	0,	60,	'G�rczewska 62/64, 01-401 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (17,'Park Skaryszewski',	'52.242342360374714,21.05470515806566',	'Du�y park, kt�rego punktem charakterystycznym jest du�e, pod�u�ne jezioro',	0,	90,	'al. Jerzego Waszyngtona, 00-999 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (18,'OchTeatr',	'52.21472532540376,20.980105379256027',	'Nowoczesny teatr z rozmaitym repertuarem',	140,	180,	'Gr�jecka 65, 02-094 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),840,840,840,840,840,840,840,1320,1320,1320,1320,1320,1320,1320)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (19,'Park Saski',	'52.24041075642384,21.007627156608375',	'Park w okolicach Pa�acu Prezydenckiego, w kt�rym mo�na obejrze� ogrody kwietne, fontann� oraz Gr�b Nieznanego �o�nierza',	0,	45,	'Marsza�kowska, 00-102 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (20,'Multimedialny Park Fontann',	'52.25408484749848,21.01139098949795',	'Park z fontannami idealny do odpoczynku w letnie dni. Wieczorami odbywaj� si� tam pokazy �wietlno-d�wi�kowe',	0,	60,	'Skwer 1 Dywizji Pancernej WP, 00-221 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

SET IDENTITY_INSERT Places OFF
SELECT * FROM placeAvailabilityTimes
SELECT * FROM Places