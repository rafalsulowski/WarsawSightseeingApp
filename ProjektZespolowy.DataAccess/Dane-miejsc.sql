USE ProjektZespolowy
--use master
DELETE FROM PlacesTrips
DELETE FROM placeAvailabilityTimes
DELETE FROM Places
DELETE FROM placeAvailabilityTimes

SET IDENTITY_INSERT Places ON

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (1,'Muzeum Narodowe',	'52.2317404268188,21.024337782647017',	'Muzeum sztuki i rzeŸby zwi¹zanej z histori¹ staro¿ytna i wspó³czesn¹ Polski',	25,	120,	'al. Jerozolimskie 3, 00-495 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY()  ,0,600,600,600,600,600,600,0,1080,1080,1080,1200,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (2,'Muzeum Powstania Warszawskiego',	'52.23277747505596,20.980874792519103',	'Muzeum z wystawami przedstawiaj¹cymi przebieg powstania warszawskiego i ¿ycie osób w nim uczestnicz¹cym',	30,	120,	'Grzybowska 79, 00-844 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),480,0,480,480,480,600,600,1080,0,1080,1080,1080,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (3,'Zamek Królewski',	'52.247752464638275,21.014605598114446',	'XIV-wieczny pa³ac oferuj¹cy zwiedzanie sal oraz galerii sztuki',	23,	90,	'plac Zamkowy 4, 00-277 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,600,600,600,600,600,600,0,1020,1020,1020,1020,1020,1020)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (4,'Pa³ac Kultury i Nauki',	'52.23194394437168,21.005959090463246',	'Pa³ac wybudowany w po³owie XX wieku, w którym odbywa siê wiele wydarzeñ kulturowych. Posiada taras widokowy na 30 piêtrze.',	25,	60,	'plac Defilad 1, 00-901 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,600,1200,1200,1200,1200,1200,1200,1200)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (5,'Centrum Nauki Kopernik',	'52.242005001296334,21.028787109382893',	'Nowoczesne centrum naukowe z interaktywnymi wystawami i labolatoriami',	40,	180,	'Wybrze¿e Koœciuszkowskie 20, 00-390 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),540,540,540,540,540,540,540,1080,1080,1080,1080,1200,1140,1140)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (6,'Muzeum Fryderyka Chopina',	'52.2365339111456,21.022931471860616',	'Muzeum skupiaj¹ce siê na historii i twórczoœci Fryderyka Chopina',	25,	60,	'Pa³ac Gniñskich, Okólnik 1, 00-368 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,600,600,600,600,600,600,0,1080,1080,1080,1080,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (7,'PGE Narodowy',	'52.23942018809969,21.045250325519294',	'Stadion pi³ki no¿nej, który jest miejscem wielu wydarzeñ sportowych i wystaw. Umo¿liwia te¿ zwiedzanie z przewodnikiem',	25,	60,	'al. Ksiêcia Józefa Poniatowskiego 1, 03-901 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),660,660,660,660,660,660,660,1080,1080,1080,1080,1080,1080,1080)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (8,'£azienki Królewskie',	'52.21540461751925,21.03496890964672',	'Ogromny park niedaleko centrum miasta',	0,	60,	'Ujazdów, 00-460 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (9,'Ogrody Pa³acu Jana III w Wilanowie',	'52.16537247908525,21.09037854559097',	'Ogrody pe³ne ró¿norodnych kwiatów, drzew i krzewów ozdabiaj¹ce pa³ac królewski.',	10,	90,	'Stanis³awa Kostki Potockiego 10/16, 02-958 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (10,'Muzeum Pa³acu Jana III w Wilanowie',	'52.16537247908525,21.09037854559097',	'XVII-wieczny pa³ac zawieraj¹cy odrestaurowane sale, wystawy i galerie sztuki',	35,	180,	'Stanis³awa Kostki Potockiego 10/16, 02-958 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,600,960,960,960,960,960,960,960)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (11,'Z³ote Tarasy',	'52.23017829799075,21.002781792758437',	'Galeria handlowa mieszcz¹ce siê w samym centrum miasta. Zawiera m.in. sklepy odzie¿owe, restauracje, skelpy spo¿ywcze.',	0,	60,	'Z³ota 59, 00-120 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),540,540,540,540,540,540,0,1320,1320,1320,1320,1320,1320,0)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (12,'Westfield Arkadia',	'52.25734712517806,20.984648511452917',	'Jedna z najwiêkszych galerii handlowych w mieœcie',	0,	60,	'al. Jana Paw³a II 82, 00-175 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,0,1320,1320,1320,1320,1320,1320,0)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (13,'Westfield Mokotów',	'52.18061362356889,21.005435017212694',	'Najwiêksza galeria handlowe w po³udniowej czêœci Warszawy',	0,	60,	'Wo³oska 12, 02-675 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),600,600,600,600,600,600,0,1320,1320,1320,1320,1320,1320,0)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (14,'Park Szczêœliwicki',	'52.20492644301373,20.95955463061865',	'Du¿y park, w którym mo¿na odpocz¹æ nad jeziorem lub zjechaæ na nartach ze sztucznego stoku',	0,	90,	'02-384 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId,OpeningMonday, OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (15,'Park Edwarda Szymañskiego',	'52.2340116991027,20.951806671971315',	'Nowoczesny park z wieloma œcie¿kami dla pieszych, rowerów oraz deskorolek',	0,	60,	'Elekcyjna, Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (16,'Park Moczyd³o',	'52.24213165848122,20.95169501148924',	'Park zawieraj¹cy stawy, bazar oraz górkê z widokiem na panoramê centrum miasta',	0,	60,	'Górczewska 62/64, 01-401 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (17,'Park Skaryszewski',	'52.242342360374714,21.05470515806566',	'Du¿y park, którego punktem charakterystycznym jest du¿e, pod³u¿ne jezioro',	0,	90,	'al. Jerzego Waszyngtona, 00-999 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (18,'OchTeatr',	'52.21472532540376,20.980105379256027',	'Nowoczesny teatr z rozmaitym repertuarem',	140,	180,	'Grójecka 65, 02-094 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),840,840,840,840,840,840,840,1320,1320,1320,1320,1320,1320,1320)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (19,'Park Saski',	'52.24041075642384,21.007627156608375',	'Park w okolicach Pa³acu Prezydenckiego, w którym mo¿na obejrzeæ ogrody kwietne, fontannê oraz Grób Nieznanego ¯o³nierza',	0,	45,	'Marsza³kowska, 00-102 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

INSERT INTO Places(Id,Name, Coordinates, Description, EstimatedCost, AverageTimeSpent, Address)
VALUES (20,'Multimedialny Park Fontann',	'52.25408484749848,21.01139098949795',	'Park z fontannami idealny do odpoczynku w letnie dni. Wieczorami odbywaj¹ siê tam pokazy œwietlno-dŸwiêkowe',	0,	60,	'Skwer 1 Dywizji Pancernej WP, 00-221 Warszawa')
INSERT INTO placeAvailabilityTimes(PlaceId, OpeningMonday,OpeningTuesday, OpeningWednesday, OpeningThursday, OpeningFriday, OpeningSaturday, OpeningSunday, ClosingMonday, ClosingTuesday, ClosingWednesday, ClosingThursday, ClosingFriday, ClosingSaturday, ClosingSunday)
VALUES (SCOPE_IDENTITY(),0,0,0,0,0,0,0,1440,1440,1440,1440,1440,1440,1440)

SET IDENTITY_INSERT Places OFF
SELECT * FROM placeAvailabilityTimes
SELECT * FROM Places