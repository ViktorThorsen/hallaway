create sequence hotel_hotel_id_seq;

alter sequence hotel_hotel_id_seq owner to postgres;

alter sequence hotel_hotel_id_seq owned by "Hotel".hotel_id;

