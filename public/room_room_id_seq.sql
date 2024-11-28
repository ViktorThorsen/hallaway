create sequence room_room_id_seq;

alter sequence room_room_id_seq owner to postgres;

alter sequence room_room_id_seq owned by "Room".room_id;

