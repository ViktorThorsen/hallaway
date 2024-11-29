--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4
-- Dumped by pg_dump version 16.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: bid4wheelsdb; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA bid4wheelsdb;


ALTER SCHEMA bid4wheelsdb OWNER TO postgres;

--
-- Name: adminpack; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION adminpack; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: vehicle; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb.vehicle (
    id integer NOT NULL,
    model text,
    year text,
    milage text,
    fuel text,
    gearbox text,
    is_sold boolean,
    category integer NOT NULL
);


ALTER TABLE bid4wheelsdb.vehicle OWNER TO postgres;

--
-- Name: add_vehicle; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.add_vehicle AS
 SELECT model,
    year,
    milage,
    fuel,
    gearbox,
    is_sold,
    category
   FROM bid4wheelsdb.vehicle;


ALTER VIEW bid4wheelsdb.add_vehicle OWNER TO postgres;

--
-- Name: address; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb.address (
    id integer NOT NULL,
    city text,
    street text,
    zip_code text
);


ALTER TABLE bid4wheelsdb.address OWNER TO postgres;

--
-- Name: address_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb.address ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.address_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: auction; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb.auction (
    id integer NOT NULL,
    name text NOT NULL,
    end_date timestamp without time zone NOT NULL,
    start_date timestamp without time zone NOT NULL,
    start_price double precision,
    buyout double precision,
    reservation_price double precision,
    owner integer NOT NULL,
    vehicle_id integer NOT NULL
);


ALTER TABLE bid4wheelsdb.auction OWNER TO postgres;

--
-- Name: auction_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb.auction ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.auction_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: bids; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb.bids (
    id integer NOT NULL,
    auction_id integer NOT NULL,
    user_id integer NOT NULL,
    bid double precision NOT NULL,
    "time" timestamp without time zone NOT NULL
);


ALTER TABLE bid4wheelsdb.bids OWNER TO postgres;

--
-- Name: bids_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb.bids ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.bids_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: user; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb."user" (
    id integer NOT NULL,
    name text NOT NULL,
    address integer NOT NULL,
    phone text,
    email text,
    password text,
    is_admin boolean NOT NULL,
    is_blocked boolean DEFAULT false
);


ALTER TABLE bid4wheelsdb."user" OWNER TO postgres;

--
-- Name: block_user; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.block_user AS
 SELECT is_blocked,
    id
   FROM bid4wheelsdb."user";


ALTER VIEW bid4wheelsdb.block_user OWNER TO postgres;

--
-- Name: category; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb.category (
    id integer NOT NULL,
    name text
);


ALTER TABLE bid4wheelsdb.category OWNER TO postgres;

--
-- Name: category_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb.category ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: count_bids; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.count_bids AS
SELECT
    NULL::text AS model,
    NULL::bigint AS biders,
    NULL::double precision AS highest_bid;


ALTER VIEW bid4wheelsdb.count_bids OWNER TO postgres;

--
-- Name: create_account; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.create_account AS
 SELECT name,
    address,
    phone,
    email,
    password,
    is_admin
   FROM bid4wheelsdb."user";


ALTER VIEW bid4wheelsdb.create_account OWNER TO postgres;

--
-- Name: create_auction; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.create_auction AS
 SELECT name,
    end_date,
    start_date,
    start_price,
    buyout,
    reservation_price,
    owner,
    vehicle_id
   FROM bid4wheelsdb.auction;


ALTER VIEW bid4wheelsdb.create_auction OWNER TO postgres;

--
-- Name: filter_by_category; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.filter_by_category AS
 SELECT auction.name AS description,
    vehicle.model,
    category.name AS category_name,
    max(bids.bid) AS top_bid
   FROM bid4wheelsdb.auction,
    bid4wheelsdb.vehicle,
    bid4wheelsdb.category,
    bid4wheelsdb.bids
  WHERE ((auction.vehicle_id = vehicle.id) AND (vehicle.category = category.id) AND (auction.id = bids.auction_id))
  GROUP BY vehicle.model, category.name, auction.name, vehicle.category
  ORDER BY vehicle.category, (max(bids.bid));


ALTER VIEW bid4wheelsdb.filter_by_category OWNER TO postgres;

--
-- Name: make_bid; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.make_bid AS
 SELECT auction_id,
    user_id,
    bid,
    "time"
   FROM bid4wheelsdb.bids;


ALTER VIEW bid4wheelsdb.make_bid OWNER TO postgres;

--
-- Name: picture; Type: TABLE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE TABLE bid4wheelsdb.picture (
    id integer NOT NULL,
    link text NOT NULL,
    vehicle_id integer NOT NULL
);


ALTER TABLE bid4wheelsdb.picture OWNER TO postgres;

--
-- Name: picture_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb.picture ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.picture_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: search_by_city; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.search_by_city AS
 SELECT address.city,
    "user".name,
    vehicle.model,
    max(bids.bid) AS highest_bid
   FROM bid4wheelsdb.vehicle,
    bid4wheelsdb.auction,
    bid4wheelsdb."user",
    bid4wheelsdb.address,
    bid4wheelsdb.bids
  WHERE ((vehicle.id = auction.vehicle_id) AND (auction.owner = "user".id) AND ("user".address = address.id) AND (bids.auction_id = auction.id))
  GROUP BY address.city, "user".name, vehicle.model;


ALTER VIEW bid4wheelsdb.search_by_city OWNER TO postgres;

--
-- Name: sort_by_date; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.sort_by_date AS
 SELECT auction.name,
    vehicle.model,
    auction.start_date,
    auction.end_date
   FROM bid4wheelsdb.auction,
    bid4wheelsdb.vehicle
  WHERE ((auction.vehicle_id = vehicle.id) AND (vehicle.is_sold = false))
  ORDER BY auction.end_date, auction.start_date;


ALTER VIEW bid4wheelsdb.sort_by_date OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb."user" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: vehicle_id_seq; Type: SEQUENCE; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE bid4wheelsdb.vehicle ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME bid4wheelsdb.vehicle_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: view_active_auctions; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_active_auctions AS
 SELECT auction.id AS auction_id,
    auction.name,
    vehicle.model
   FROM bid4wheelsdb.auction,
    bid4wheelsdb.vehicle
  WHERE ((auction.vehicle_id = vehicle.id) AND (vehicle.is_sold = false));


ALTER VIEW bid4wheelsdb.view_active_auctions OWNER TO postgres;

--
-- Name: view_bids_on_vehicle; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_bids_on_vehicle AS
 SELECT auction.vehicle_id,
    auction.name AS auction_name,
    bids.bid,
    "user".name AS user_name
   FROM bid4wheelsdb.bids,
    bid4wheelsdb.auction,
    bid4wheelsdb."user"
  WHERE ((bids.auction_id = auction.id) AND (bids.user_id = "user".id))
  ORDER BY bids.auction_id, bids.bid DESC;


ALTER VIEW bid4wheelsdb.view_bids_on_vehicle OWNER TO postgres;

--
-- Name: view_ended_auctions; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_ended_auctions AS
 SELECT auction.id AS auction_id,
    auction.name,
    vehicle.model,
    auction.end_date
   FROM bid4wheelsdb.auction,
    bid4wheelsdb.vehicle
  WHERE ((auction.vehicle_id = vehicle.id) AND (vehicle.is_sold = true));


ALTER VIEW bid4wheelsdb.view_ended_auctions OWNER TO postgres;

--
-- Name: view_highest_bid; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_highest_bid AS
 SELECT max(bids.bid) AS higest_bid,
    "user".name,
    auction.reservation_price,
    vehicle.model
   FROM bid4wheelsdb.bids,
    bid4wheelsdb.auction,
    bid4wheelsdb.vehicle,
    bid4wheelsdb."user"
  WHERE ((bids.auction_id = auction.id) AND (auction.vehicle_id = vehicle.id) AND (bids.user_id = "user".id))
  GROUP BY vehicle.model, auction.reservation_price, auction.vehicle_id, "user".name;


ALTER VIEW bid4wheelsdb.view_highest_bid OWNER TO postgres;

--
-- Name: view_user_details; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_user_details AS
 SELECT "user".id AS user_id,
    "user".name,
    "user".phone,
    "user".email,
    address.city,
    address.street,
    address.zip_code,
    "user".is_admin
   FROM bid4wheelsdb."user",
    bid4wheelsdb.address
  WHERE ("user".address = address.id);


ALTER VIEW bid4wheelsdb.view_user_details OWNER TO postgres;

--
-- Name: view_users; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_users AS
 SELECT u.name,
    u.phone,
    u.email,
    address.city,
    address.street,
    address.zip_code,
    u.is_admin
   FROM bid4wheelsdb."user" u,
    bid4wheelsdb.address
  WHERE (u.address = address.id);


ALTER VIEW bid4wheelsdb.view_users OWNER TO postgres;

--
-- Name: view_vehicle_details; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_vehicle_details AS
 SELECT auction.name AS auction_name,
    vehicle.model,
    category.name AS category_name,
    vehicle.year,
    vehicle.milage,
    vehicle.fuel,
    vehicle.gearbox,
    auction.end_date,
    auction.start_date
   FROM bid4wheelsdb.auction,
    bid4wheelsdb.vehicle,
    bid4wheelsdb.category
  WHERE ((auction.vehicle_id = vehicle.id) AND (vehicle.category = category.id))
  GROUP BY auction.name, vehicle.model, category.name, vehicle.year, vehicle.milage, vehicle.fuel, vehicle.gearbox, auction.end_date, auction.start_date;


ALTER VIEW bid4wheelsdb.view_vehicle_details OWNER TO postgres;

--
-- Name: view_vehicles_for_sale; Type: VIEW; Schema: bid4wheelsdb; Owner: postgres
--

CREATE VIEW bid4wheelsdb.view_vehicles_for_sale AS
 SELECT auction.name,
    vehicle.model,
    max(bids.bid) AS top_bid
   FROM bid4wheelsdb.auction,
    bid4wheelsdb.vehicle,
    bid4wheelsdb.bids
  WHERE ((auction.vehicle_id = vehicle.id) AND (bids.auction_id = auction.id) AND (vehicle.is_sold IS NOT TRUE))
  GROUP BY auction.name, vehicle.model;


ALTER VIEW bid4wheelsdb.view_vehicles_for_sale OWNER TO postgres;

--
-- Name: AddOn; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AddOn" (
    addon_id integer NOT NULL,
    name character varying,
    description character varying,
    price double precision,
    hotel integer,
    "order" integer
);


ALTER TABLE public."AddOn" OWNER TO postgres;

--
-- Name: AddOn_addon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."AddOn" ALTER COLUMN addon_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."AddOn_addon_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Address; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Address" (
    location_id integer NOT NULL,
    city character varying,
    street character varying
);


ALTER TABLE public."Address" OWNER TO postgres;

--
-- Name: Address_location_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Address" ALTER COLUMN location_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Address_location_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Hotel; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Hotel" (
    hotel_id integer NOT NULL,
    hotel_name character varying,
    address integer,
    pool boolean,
    resturant boolean,
    kidsclub boolean,
    rating integer,
    distancebeach integer,
    distancecitycenter integer,
    evningentertainment boolean
);


ALTER TABLE public."Hotel" OWNER TO postgres;

--
-- Name: Hotel_hotel_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Hotel" ALTER COLUMN hotel_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Hotel_hotel_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Order; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Order" (
    order_id integer NOT NULL,
    party integer,
    admin integer,
    hotel integer,
    date date,
    totalprice double precision
);


ALTER TABLE public."Order" OWNER TO postgres;

--
-- Name: Order_order_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Order" ALTER COLUMN order_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Order_order_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Party; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Party" (
    id integer NOT NULL,
    organizer_id integer
);


ALTER TABLE public."Party" OWNER TO postgres;

--
-- Name: Party_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Party" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Party_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Person; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Person" (
    user_id integer NOT NULL,
    name character varying,
    phone character varying,
    email character varying,
    date_of_birth date,
    party_id integer
);


ALTER TABLE public."Person" OWNER TO postgres;

--
-- Name: Person_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Person" ALTER COLUMN user_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Person_user_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Room; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Room" (
    room_id integer NOT NULL,
    price double precision,
    size integer,
    hotel_id integer
);


ALTER TABLE public."Room" OWNER TO postgres;

--
-- Name: Room_room_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Room" ALTER COLUMN room_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Room_room_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: addon_addon_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.addon_addon_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.addon_addon_id_seq OWNER TO postgres;

--
-- Name: address_location_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.address_location_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.address_location_id_seq OWNER TO postgres;

--
-- Name: address_location_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.address_location_id_seq OWNED BY public."Address".location_id;


--
-- Name: admin; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin (
    admin_id integer NOT NULL,
    name character varying,
    phone character varying,
    email character varying,
    date_of_birth date
);


ALTER TABLE public.admin OWNER TO postgres;

--
-- Name: admin_admin_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.admin ALTER COLUMN admin_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.admin_admin_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: hotel_hotel_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.hotel_hotel_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.hotel_hotel_id_seq OWNER TO postgres;

--
-- Name: hotel_hotel_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.hotel_hotel_id_seq OWNED BY public."Hotel".hotel_id;


--
-- Name: party_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.party_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.party_id_seq OWNER TO postgres;

--
-- Name: person_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.person_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.person_user_id_seq OWNER TO postgres;

--
-- Name: person_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.person_user_id_seq OWNED BY public."Person".user_id;


--
-- Name: room_room_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.room_room_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.room_room_id_seq OWNER TO postgres;

--
-- Name: room_room_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.room_room_id_seq OWNED BY public."Room".room_id;


--
-- Data for Name: address; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb.address (id, city, street, zip_code) FROM stdin;
1	Malmö	Storgatan 1	41111
2	Halmstad	Stora Torg 11	31393
3	Norrköping	Frivaktsgatan 4	41334
\.


--
-- Data for Name: auction; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb.auction (id, name, end_date, start_date, start_price, buyout, reservation_price, owner, vehicle_id) FROM stdin;
2	Din nya Peageot	2024-09-30 15:00:00	2024-09-01 11:35:12	\N	229900	\N	1	2
1	Sista Toyotan du köper	2024-10-22 17:00:00	2024-03-24 09:00:00	170000	368500	340000	2	1
3	Jaggan för dig	2025-03-24 15:00:28	2024-09-14 15:01:28	200000	\N	400000	3	3
12	Honda CR-V Deal	2024-12-15 18:00:00	2024-09-25 09:30:00	140000	300000	250000	1	4
13	Tesla Model 3 Auction	2024-11-20 12:00:00	2024-10-01 10:00:00	250000	450000	400000	2	5
14	Ford Mustang Classic	2024-10-05 14:00:00	2024-08-15 12:00:00	180000	380000	350000	2	6
15	BMW X5 Special	2024-11-01 10:30:00	2024-09-20 09:00:00	230000	\N	320000	2	7
16	Audi e-tron Exclusive	2024-12-05 17:00:00	2024-10-10 13:45:00	260000	480000	450000	1	8
17	Mercedes-Benz GLC Auction	2024-11-30 16:00:00	2024-09-15 10:15:00	220000	400000	360000	3	9
18	Volkswagen ID.4 Bid	2025-01-10 13:00:00	2024-10-05 08:00:00	210000	390000	350000	2	10
19	Nissan Leaf Bargain	2024-12-25 15:30:00	2024-09-20 12:00:00	160000	340000	300000	2	11
\.


--
-- Data for Name: bids; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb.bids (id, auction_id, user_id, bid, "time") FROM stdin;
1	1	1	170000	2024-09-24 15:23:28
2	1	3	180000	2024-09-24 16:01:28
3	2	2	229900	2024-09-24 15:12:28
4	3	1	200000	2024-09-24 16:01:28
5	3	2	211000	2024-09-24 16:12:28
6	3	1	240000	2024-09-24 16:22:28
7	3	2	300000	2024-09-24 16:48:28
19	1	2	185000	2024-09-25 09:30:00
20	1	1	195000	2024-09-25 10:15:00
21	2	3	235000	2024-09-26 12:00:00
22	2	1	240000	2024-09-26 14:05:00
23	3	3	310000	2024-09-26 15:30:00
24	12	2	150000	2024-09-27 10:00:00
25	12	1	160000	2024-09-27 11:15:00
26	13	3	260000	2024-09-27 13:30:00
27	13	1	280000	2024-09-27 14:00:00
28	14	2	190000	2024-09-28 09:00:00
29	14	1	195000	2024-09-28 10:45:00
\.


--
-- Data for Name: category; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb.category (id, name) FROM stdin;
1	SUV
2	Combi
3	Sedan
4	Sport
\.


--
-- Data for Name: picture; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb.picture (id, link, vehicle_id) FROM stdin;
1	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.toyota.se%2Fbilar%2Frav4&psig=AOvVaw3Hq3JabJ_nD8MW_GDW5017&ust=1727271088047000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCICD5bHY24gDFQAAAAAdAAAAABAE	1
2	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.toyota.se%2Fbilar%2Frav4&psig=AOvVaw3Hq3JabJ_nD8MW_GDW5017&ust=1727271088047000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCICD5bHY24gDFQAAAAAdAAAAABAI	1
3	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.edmunds.com%2Ftoyota%2Frav4-hybrid%2F&psig=AOvVaw3Hq3JabJ_nD8MW_GDW5017&ust=1727271088047000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCICD5bHY24gDFQAAAAAdAAAAABAP	1
4	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.bytbil.com%2Fstockholms-lan%2Fpersonbil-2008-gt-puretech-130hk-aut-demo-2718-17541214&psig=AOvVaw10m0gWw2gyimLL-hbliEeT&ust=1727271215737000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCNj4me3Y24gDFQAAAAAdAAAAABAE	2
5	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.peugeot.dk%2Fkampagner%2Faktuelle-kampagner%2F2008-kampagner.html&psig=AOvVaw10m0gWw2gyimLL-hbliEeT&ust=1727271215737000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCNj4me3Y24gDFQAAAAAdAAAAABAJ	2
6	https://www.google.com/url?sa=i&url=https%3A%2F%2Fmedia.jaguar.com%2F2018%2Fnew-jaguar-i-pace-electrifying-design&psig=AOvVaw0OYeUTLZc0gpfL8fT12wAn&ust=1727271307822000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKixspjZ24gDFQAAAAAdAAAAABAE	3
7	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.kbb.com%2Fjaguar%2Fi-pace%2F&psig=AOvVaw0OYeUTLZc0gpfL8fT12wAn&ust=1727271307822000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKixspjZ24gDFQAAAAAdAAAAABAJ	3
8	https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.albioncars.cz%2Fpredvadeci-skladovy-vuz-jaguar-i-pace-49549081&psig=AOvVaw0OYeUTLZc0gpfL8fT12wAn&ust=1727271307822000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKixspjZ24gDFQAAAAAdAAAAABAP	3
9	https://www.google.com/url?sa=i&url=https%3A%2F%2Ftr.motor1.com%2Fnews%2F430299%2F2021-jaguar-i-pace-guncelleme%2F&psig=AOvVaw0OYeUTLZc0gpfL8fT12wAn&ust=1727271307822000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKixspjZ24gDFQAAAAAdAAAAABAV	3
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb."user" (id, name, address, phone, email, password, is_admin, is_blocked) FROM stdin;
1	Benjamin Berglund	1	0702784174	benjamin@nodehill.com	benji123	f	f
3	Daniel Theoren	3	0736109183	daniel.theoren@gmail.com	danne123	t	f
2	Karl Bilt	2	0720345776	kalle@rosenbad.se	biltan321	f	t
\.


--
-- Data for Name: vehicle; Type: TABLE DATA; Schema: bid4wheelsdb; Owner: postgres
--

COPY bid4wheelsdb.vehicle (id, model, year, milage, fuel, gearbox, is_sold, category) FROM stdin;
1	Toyota RAV4	2021	5007	petrol	automatic	f	1
2	Peugeot 2008	2021	3798	electric	automatic	f	2
3	Jaguar I-Pace	2019	6402	electric	automatic	t	4
4	Honda CR-V	2020	12500	petrol	automatic	f	1
5	Tesla Model 3	2022	3100	electric	automatic	f	2
6	Ford Mustang	2018	8750	petrol	manual	t	3
7	BMW X5	2019	9800	diesel	automatic	f	1
8	Audi e-tron	2021	4300	electric	automatic	f	4
9	Mercedes-Benz GLC	2020	6700	petrol	automatic	f	1
10	Volkswagen ID.4	2022	1500	electric	automatic	f	2
11	Nissan Leaf	2019	8500	electric	automatic	t	2
\.


--
-- Data for Name: AddOn; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AddOn" (addon_id, name, description, price, hotel, "order") FROM stdin;
\.


--
-- Data for Name: Address; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Address" (location_id, city, street) FROM stdin;
\.


--
-- Data for Name: Hotel; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Hotel" (hotel_id, hotel_name, address, pool, resturant, kidsclub, rating, distancebeach, distancecitycenter, evningentertainment) FROM stdin;
\.


--
-- Data for Name: Order; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Order" (order_id, party, admin, hotel, date, totalprice) FROM stdin;
\.


--
-- Data for Name: Party; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Party" (id, organizer_id) FROM stdin;
\.


--
-- Data for Name: Person; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Person" (user_id, name, phone, email, date_of_birth, party_id) FROM stdin;
\.


--
-- Data for Name: Room; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Room" (room_id, price, size, hotel_id) FROM stdin;
\.


--
-- Data for Name: admin; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.admin (admin_id, name, phone, email, date_of_birth) FROM stdin;
\.


--
-- Name: address_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.address_id_seq', 12, true);


--
-- Name: auction_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.auction_id_seq', 19, true);


--
-- Name: bids_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.bids_id_seq', 29, true);


--
-- Name: category_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.category_id_seq', 4, true);


--
-- Name: picture_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.picture_id_seq', 9, true);


--
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.user_id_seq', 3, true);


--
-- Name: vehicle_id_seq; Type: SEQUENCE SET; Schema: bid4wheelsdb; Owner: postgres
--

SELECT pg_catalog.setval('bid4wheelsdb.vehicle_id_seq', 11, true);


--
-- Name: AddOn_addon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AddOn_addon_id_seq"', 1, false);


--
-- Name: Address_location_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Address_location_id_seq"', 1, false);


--
-- Name: Hotel_hotel_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Hotel_hotel_id_seq"', 1, false);


--
-- Name: Order_order_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Order_order_id_seq"', 1, false);


--
-- Name: Party_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Party_id_seq"', 1, false);


--
-- Name: Person_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Person_user_id_seq"', 1, false);


--
-- Name: Room_room_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Room_room_id_seq"', 1, false);


--
-- Name: addon_addon_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.addon_addon_id_seq', 1, false);


--
-- Name: address_location_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.address_location_id_seq', 1, false);


--
-- Name: admin_admin_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.admin_admin_id_seq', 1, false);


--
-- Name: hotel_hotel_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.hotel_hotel_id_seq', 1, false);


--
-- Name: party_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.party_id_seq', 1, false);


--
-- Name: person_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.person_user_id_seq', 1, false);


--
-- Name: room_room_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.room_room_id_seq', 1, false);


--
-- Name: address address_pk; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.address
    ADD CONSTRAINT address_pk PRIMARY KEY (id);


--
-- Name: auction auction_pk; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.auction
    ADD CONSTRAINT auction_pk PRIMARY KEY (id);


--
-- Name: bids bids_pk; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.bids
    ADD CONSTRAINT bids_pk PRIMARY KEY (id);


--
-- Name: category category_pkey; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.category
    ADD CONSTRAINT category_pkey PRIMARY KEY (id);


--
-- Name: picture picture_pk; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.picture
    ADD CONSTRAINT picture_pk PRIMARY KEY (id);


--
-- Name: user user_pk; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb."user"
    ADD CONSTRAINT user_pk PRIMARY KEY (id);


--
-- Name: vehicle vehicle_pk; Type: CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.vehicle
    ADD CONSTRAINT vehicle_pk PRIMARY KEY (id);


--
-- Name: AddOn addon_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AddOn"
    ADD CONSTRAINT addon_pk PRIMARY KEY (addon_id);


--
-- Name: Address address_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Address"
    ADD CONSTRAINT address_pk PRIMARY KEY (location_id);


--
-- Name: admin admin_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin
    ADD CONSTRAINT admin_pk PRIMARY KEY (admin_id);


--
-- Name: Hotel hotel_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Hotel"
    ADD CONSTRAINT hotel_pk PRIMARY KEY (hotel_id);


--
-- Name: Party id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Party"
    ADD CONSTRAINT id PRIMARY KEY (id);


--
-- Name: Order order_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT order_pk PRIMARY KEY (order_id);


--
-- Name: Person person_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Person"
    ADD CONSTRAINT person_pk PRIMARY KEY (user_id);


--
-- Name: Room room_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Room"
    ADD CONSTRAINT room_pk PRIMARY KEY (room_id);


--
-- Name: address_city_street_zip_code_index; Type: INDEX; Schema: bid4wheelsdb; Owner: postgres
--

CREATE INDEX address_city_street_zip_code_index ON bid4wheelsdb.address USING btree (city, street, zip_code);


--
-- Name: user_address_uindex; Type: INDEX; Schema: bid4wheelsdb; Owner: postgres
--

CREATE UNIQUE INDEX user_address_uindex ON bid4wheelsdb."user" USING btree (address);


--
-- Name: count_bids _RETURN; Type: RULE; Schema: bid4wheelsdb; Owner: postgres
--

CREATE OR REPLACE VIEW bid4wheelsdb.count_bids AS
 SELECT vehicle.model,
    count(DISTINCT bids.user_id) AS biders,
    max(bids.bid) AS highest_bid
   FROM bid4wheelsdb.vehicle,
    bid4wheelsdb.auction,
    bid4wheelsdb.bids
  WHERE ((auction.vehicle_id = vehicle.id) AND (bids.auction_id = auction.id))
  GROUP BY auction.id, vehicle.id;


--
-- Name: auction auction_user_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.auction
    ADD CONSTRAINT auction_user_id_fk FOREIGN KEY (owner) REFERENCES bid4wheelsdb."user"(id);


--
-- Name: auction auction_vehicle_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.auction
    ADD CONSTRAINT auction_vehicle_id_fk FOREIGN KEY (vehicle_id) REFERENCES bid4wheelsdb.vehicle(id);


--
-- Name: bids bids_auction_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.bids
    ADD CONSTRAINT bids_auction_id_fk FOREIGN KEY (auction_id) REFERENCES bid4wheelsdb.auction(id);


--
-- Name: bids bids_user_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.bids
    ADD CONSTRAINT bids_user_id_fk FOREIGN KEY (user_id) REFERENCES bid4wheelsdb."user"(id);


--
-- Name: picture picture_vehicle_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.picture
    ADD CONSTRAINT picture_vehicle_id_fk FOREIGN KEY (vehicle_id) REFERENCES bid4wheelsdb.vehicle(id);


--
-- Name: user user_address_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb."user"
    ADD CONSTRAINT user_address_id_fk FOREIGN KEY (address) REFERENCES bid4wheelsdb.address(id);


--
-- Name: vehicle vehicle_category_id_fk; Type: FK CONSTRAINT; Schema: bid4wheelsdb; Owner: postgres
--

ALTER TABLE ONLY bid4wheelsdb.vehicle
    ADD CONSTRAINT vehicle_category_id_fk FOREIGN KEY (category) REFERENCES bid4wheelsdb.category(id);


--
-- Name: AddOn addon_hotel_hotel_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AddOn"
    ADD CONSTRAINT addon_hotel_hotel_id_fk FOREIGN KEY (hotel) REFERENCES public."Hotel"(hotel_id);


--
-- Name: AddOn addon_order_order_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AddOn"
    ADD CONSTRAINT addon_order_order_id_fk FOREIGN KEY ("order") REFERENCES public."Order"(order_id);


--
-- Name: Hotel hotel_address_location_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Hotel"
    ADD CONSTRAINT hotel_address_location_id_fk FOREIGN KEY (address) REFERENCES public."Address"(location_id);


--
-- Name: Order order_admin_admin_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT order_admin_admin_id_fk FOREIGN KEY (admin) REFERENCES public.admin(admin_id);


--
-- Name: Order order_hotel_hotel_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT order_hotel_hotel_id_fk FOREIGN KEY (party) REFERENCES public."Hotel"(hotel_id);


--
-- Name: Order order_hotel_hotel_id_fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT order_hotel_hotel_id_fk_2 FOREIGN KEY (hotel) REFERENCES public."Hotel"(hotel_id);


--
-- Name: Order order_party_id_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT order_party_id_fk FOREIGN KEY (party) REFERENCES public."Party"(id);


--
-- Name: Party organizer; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Party"
    ADD CONSTRAINT organizer FOREIGN KEY (organizer_id) REFERENCES public."Person"(user_id);


--
-- PostgreSQL database dump complete
--

