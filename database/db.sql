CREATE SCHEMA `url-shortener` ;

CREATE TABLE `url_shortener`.`url_master` (
  `id` VARCHAR(36) NOT NULL,
  `code` VARCHAR(6) NULL,
  `url` VARCHAR(100) NULL,
  `shortUrl` VARCHAR(25) NULL,
  `createdOn` DATETIME NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `code_UNIQUE` (`code` ASC) VISIBLE,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE);
