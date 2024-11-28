create table "Order"
(
    order_id   integer generated always as identity
        constraint order_pk
            primary key,
    party      integer
        constraint order_hotel_hotel_id_fk
            references "Hotel"
        constraint order_party_id_fk
            references "Party",
    admin      integer
        constraint order_admin_admin_id_fk
            references admin,
    hotel      integer
        constraint order_hotel_hotel_id_fk_2
            references "Hotel",
    date       date,
    totalprice double precision
);

alter table "Order"
    owner to postgres;

