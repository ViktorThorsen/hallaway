create table "Hotel"
(
    hotel_id            integer generated always as identity
        constraint hotel_pk
            primary key,
    hotel_name          varchar,
    address             integer
        constraint hotel_address_location_id_fk
            references "Address",
    pool                boolean,
    resturant           boolean,
    kidsclub            boolean,
    rating              integer,
    distancebeach       integer,
    distancecitycenter  integer,
    evningentertainment boolean
);

alter table "Hotel"
    owner to postgres;

