CREATE TABLE dbo."Equipment"
(
    "EquipmentId" integer NOT NULL DEFAULT nextval('dbo."Equipment_EquipmentId_seq"'::regclass),
    "TypeEquipment" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "ModelEquipment" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "DateAcquisition" timestamp without time zone NOT NULL,
    "ValueAcquisition" numeric(18,2) NOT NULL,
    "Code" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "PathImage" character varying(100) COLLATE pg_catalog."default",
    "DateRegister" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_dbo.Equipment" PRIMARY KEY ("EquipmentId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE dbo."Equipment"
    OWNER to postgres;

-- Index: Equipment_IX_Code

-- DROP INDEX dbo."Equipment_IX_Code";

CREATE UNIQUE INDEX "Equipment_IX_Code"
    ON dbo."Equipment" USING btree
    ("Code" COLLATE pg_catalog."default")
    TABLESPACE pg_default;