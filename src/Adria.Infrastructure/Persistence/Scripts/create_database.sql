/* 
ADD ALL NECESSARY TABLES, SEED DATA and POC DATA 
HINT: start with dropping tables if they exist
See example server for reference
*/

SET
FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS `pushSubscriptions`;
DROP TABLE IF EXISTS `healthAnalyse`;
DROP TABLE IF EXISTS `analyse`;
DROP TABLE IF EXISTS `bodyStats`;
DROP TABLE IF EXISTS `orderSupplementDetails`;
DROP TABLE IF EXISTS `healthAnalyses`;
DROP TABLE IF EXISTS `foodCompositions`;
DROP TABLE IF EXISTS `scans`;
DROP TABLE IF EXISTS `orders`;
DROP TABLE IF EXISTS `supplements`;
DROP TABLE IF EXISTS `foods`;
DROP TABLE IF EXISTS `nutrients`;
DROP TABLE IF EXISTS `users`;
DROP TABLE IF EXISTS `subscriptions`;

SET
FOREIGN_KEY_CHECKS = 1;

CREATE TABLE `subscriptions`
(
    `id`         VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`       VARCHAR(100)            NOT NULL,
    `PricePerMonth` DOUBLE NOT NULL,
    `Advantages` TEXT,
    `StartDate`  DATETIME                NOT NULL,
    `EndDate`    DATETIME                NOT NULL
);

CREATE TABLE `users`
(
    `AdrianId`       VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`           VARCHAR(255)            NOT NULL,
    `Job`            VARCHAR(255),
    `SubscriptionId` VARCHAR(36)             NOT NULL,
    CONSTRAINT `fk_users_subscription`
        FOREIGN KEY (`SubscriptionId`) REFERENCES `subscriptions` (`id`)
);
CREATE TABLE `pushSubscriptions`
(
    `SubscriptionId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `UserId`         VARCHAR(36)             NOT NULL,
    `Endpoint`       TEXT                    NOT NULL,
    `P256dh`         VARCHAR(255)            NOT NULL,
    `Auth`           VARCHAR(255)            NOT NULL,
    FOREIGN KEY (`UserId`) REFERENCES `users` (`AdrianId`)
);
CREATE TABLE `foods`
(
    `FoodId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`   VARCHAR(255)            NOT NULL,
    `Type`   VARCHAR(100)            NOT NULL,
    `Edible` BOOLEAN                 NOT NULL DEFAULT TRUE
);

CREATE TABLE `supplements`
(
    `SupplementId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Name`         VARCHAR(255)            NOT NULL,
    `Type`         VARCHAR(100)            NOT NULL,
    `Price` DOUBLE NOT NULL,
    `Stock`        INT                     NOT NULL DEFAULT 0
);

CREATE TABLE `nutrients`
(
    `NutrientId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Type`       VARCHAR(100)            NOT NULL,
    `Amount` DOUBLE NOT NULL         NOT NULL,
    `Unit`       VARCHAR(36),
    `Minimum`    VARCHAR(36)             NOT NULL,
    `Maximum`    VARCHAR(36)             NOT NULL
);

CREATE TABLE `orders`
(
    `OrderId`    VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`   VARCHAR(36)             NOT NULL,
    `Date`       DATETIME                NOT NULL,
    `TotalPrice` DOUBLE                  NOT NULL,
    CONSTRAINT `fk_orders_user`
        FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`)
);

CREATE TABLE `scans`
(
    `ScanId`   VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId` VARCHAR(36)             NOT NULL,
    `DateTime` DATETIME                NOT NULL,
    `Result`   TEXT,
    `FoodId`   VARCHAR(36)             NOT NULL,
    CONSTRAINT `fk_scans_user`
        FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`),
    CONSTRAINT `fk_scans_food`
        FOREIGN KEY (`FoodId`) REFERENCES `foods` (`FoodId`)
);

CREATE TABLE `foodCompositions`
(
    `FoodId`     VARCHAR(36) NOT NULL,
    `NutrientId` VARCHAR(36) NOT NULL,
    `Amount` DOUBLE NOT NULL,
    PRIMARY KEY (`FoodId`, `NutrientId`),
    CONSTRAINT `fk_foodcomp_food`
        FOREIGN KEY (`FoodId`) REFERENCES `foods` (`FoodId`),
    CONSTRAINT `fk_foodcomp_nutrient`
        FOREIGN KEY (`NutrientId`) REFERENCES `nutrients` (`NutrientId`)
);

CREATE TABLE `healthAnalyses`
(
    `AnalyseId`      VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`       VARCHAR(36)             NOT NULL,
    `DateTime`       DATETIME                NOT NULL,
    `Status`         VARCHAR(100)            NOT NULL,
    `Recommendation` TEXT,
    CONSTRAINT `fk_healthanalyses_user`
        FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`)
);

CREATE TABLE `orderSupplementDetails`
(
    `OrderId`      VARCHAR(36) NOT NULL,
    `SupplementId` VARCHAR(36) NOT NULL,
    `Amount`       VARCHAR(50) NOT NULL,
    PRIMARY KEY (`OrderId`, `SupplementId`),
    CONSTRAINT `fk_ordersupp_order`
        FOREIGN KEY (`OrderId`) REFERENCES `orders` (`OrderId`),
    CONSTRAINT `fk_ordersupp_supplement`
        FOREIGN KEY (`SupplementId`) REFERENCES `supplements` (`SupplementId`)
);


CREATE TABLE `bodyStats`
(
    `BodyStatId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `Label`      VARCHAR(100)            NOT NULL,
    `Unit`       VARCHAR(20),
    `Goal` DOUBLE
);

CREATE TABLE `analyse`
(
    `AnalyseId` VARCHAR(36) PRIMARY KEY NOT NULL,
    `AdrianId`  VARCHAR(36)             NOT NULL,
    `DateTime`  DATETIME                NOT NULL,
    FOREIGN KEY (`AdrianId`) REFERENCES `users` (`AdrianId`)
);

CREATE TABLE `healthAnalyse`
(
    `AnalyseId`  VARCHAR(36) NOT NULL,
    `BodyStatId` VARCHAR(36) NOT NULL,
    `Current` DOUBLE NOT NULL,
    PRIMARY KEY (`AnalyseId`, `BodyStatId`),
    FOREIGN KEY (`AnalyseId`) REFERENCES `analyse` (`AnalyseId`),
    FOREIGN KEY (`BodyStatId`) REFERENCES `bodyStats` (`BodyStatId`)
);

/* ========== SEED DATA ========== */

-- Insert Subscriptions
INSERT INTO subscriptions (id, `Type`, `PricePerMonth`, `Advantages`, `StartDate`, `EndDate`)
VALUES ('a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', 'Basic', 9.99, 'Access to food database, Basic scanning features','2025-01-01 00:00:00', '2026-01-01 00:00:00'),
       ('b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', 'Premium', 19.99, 'Unlimited scans, Health analysis, Personalized recommendations, Priority support', '2025-01-01 00:00:00', '2026-01-01 00:00:00'),
       ('c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', 'Free', 0.00, 'Limited food database access, 5 scans per month', '2025-01-01 00:00:00', '2026-01-01 00:00:00');

-- Insert Users
INSERT INTO users (`AdrianId`, `Name`, `Job`, `SubscriptionId`)
VALUES ('d4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', 'Adrian Martinez', 'Software Engineer', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e'),
       ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'Sarah Johnson', 'Fitness Trainer', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e'),
       ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'Michael Chen', 'Student', 'c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f'),
       ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'Emma Wilson', 'Nutritionist', 'b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e'),
       ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'David Brown', 'Chef', 'a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d');

-- Insert Nutrients
INSERT INTO nutrients (`NutrientId`, `Type`, `Amount`, `Unit`, `Minimum`, `Maximum`)
VALUES ('bd1-prot-0001', 'Protein', 50, 'g', '45', '60'),
       ('bd1-carb-0002', 'Carbohydrates', 300, 'g', '200', '350'),
       ('bd1-fats-0003', 'Fats', 25, 'g', '20', '35'),
       ('bd1-watr-0004', 'Water', 90, 'g', '70', '120'),
       ('bd1-cali-0005', 'Calories', 1000, 'g', '2000', '3000'),
       ('bd2-calc-0001', 'Calcium', 1000, 'g', '800', '1500'),
       ('bd2-iron-0002', 'Iron', 18, 'g', '8', '27'),
       ('bd2-magn-0003', 'Magnesium', 600, 'g', '400', '800'),
       ('bd2-phos-0004', 'Phosphorus', 600, 'g', '400', '800'),
       ('bd2-pota-0005', 'Potassium', 600, 'g', '400', '800'),
       ('bd2-sodi-0006', 'Sodium', 600, 'g', '400', '800'),
       ('bd2-zinc-0007', 'Zinc', 600, 'g', '400', '800'),
       ('bd2-copp-0008', 'Copper', 600, 'g', '400', '800'),
       ('bd2-mang-0009', 'Manganese', 600, 'g', '400', '800'),
       ('bd2-sele-0010', 'Selenium', 600, 'g', '400', '800'),
       ('bd2-iodi-0011', 'Iodine', 600, 'g', '400', '800');
-- Insert Foods
INSERT INTO foods (`FoodId`, `Name`, `Type`, `Edible`)
VALUES ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'Apple', 'Fruit', TRUE),
       ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'Banana', 'Fruit', TRUE),
       ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'Chicken Breast', 'Protein', TRUE),
       ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'Brown Rice', 'Grain', TRUE),
       ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'Broccoli', 'Vegetable', TRUE),
       ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'Salmon', 'Protein', TRUE),
       ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'Milk', 'Dairy', TRUE),
       ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'Spinach', 'Vegetable', TRUE),
       ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'Almonds', 'Nuts', TRUE),
       ('b6c7d8e9-f0a1-4b5c-3d4e-6f7a8b9c0d1e', 'Plastic Wrapper', 'Packaging', FALSE),
       ('e160f270-05d7-4775-b9ca-5d0e896b647e', 'Egg (whole)', 'Protein', TRUE),
       ('fdbf9c99-427f-47c2-b74e-84b08077bd4d', 'Whole Wheat Bread', 'Grain', TRUE),
       ('220307e1-39b9-45e5-b827-1ce425571e40', 'Quinoa (cooked)', 'Grain', TRUE),
       ('f24940ac-629a-46f4-9343-447d03e9fd52', 'Sweet Potato', 'Vegetable', TRUE),
       ('dff4517c-48eb-4dc3-aa56-5754f97d4131', 'Greek Yogurt', 'Dairy', TRUE),
       ('5c43ecc0-9f9d-40b7-bbd1-1f401cdb2b20', 'Cheddar Cheese', 'Dairy', TRUE),
       ('2541c962-a321-4210-a3d3-55d9771664d5', 'Carrots', 'Vegetable', TRUE),
       ('b07683ca-62d5-43a2-a06a-98cbe170394c', 'Rolled Oats', 'Grain', TRUE),
       ('34a8dd89-d4fa-455a-9212-008a0c2c34ad', 'Lentils (cooked)', 'Legume', TRUE),
       ('025c7c9c-2d98-493e-ac8f-e51730bc44f5', 'Avocado', 'Fruit', TRUE),
       ('40444ec3-94fa-4028-b880-0f7a03379e1a', 'Cardboard Box', 'Packaging', FALSE);

-- Insert FoodCompositions

INSERT INTO subscriptions (id, `Type`, `PricePerMonth`, `Advantages`, `StartDate`, `EndDate`)
VALUES
        -- Apple composition
        ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'bd1-carb-0002', 25),
        ('e7f8a9b0-c1d2-4e5f-4a5b-7c8d9e0f1a2b', 'bd1-cali-0005', 52),
        
        -- Banana
        ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'bd1-carb-0002', 27),
        ('f8a9b0c1-d2e3-4f5a-5b6c-8d9e0f1a2b3c', 'bd1-cali-0005', 89),
        
        -- Chicken Breast
        ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'bd1-prot-0001', 31),
        ('a9b0c1d2-e3f4-4a5b-6c7d-9e0f1a2b3c4d', 'bd1-cali-0005', 165),
        
        -- Brown Rice
        ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'bd1-carb-0002', 77),
        ('b0c1d2e3-f4a5-4b5c-7d8e-0f1a2b3c4d5e', 'bd1-cali-0005', 123),
        
        -- Broccoli
        ('c1d2e3f4-a5b6-4c5d-8e9f-1a2b3c4d5e6f', 'bd1-cali-0005', 34),
        
        -- Salmon
        ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'bd1-prot-0001', 20),
        ('d2e3f4a5-b6c7-4d5e-9f0a-2b3c4d5e6f7a', 'bd1-cali-0005', 208),
        
        -- Milk
        ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'bd1-prot-0001', 3.4),
        ('e3f4a5b6-c7d8-4e5f-0a1b-3c4d5e6f7a8b', 'bd1-cali-0005', 42),
        
        -- Spinach
        ('f4a5b6c7-d8e9-4f5a-1b2c-4d5e6f7a8b9c', 'bd1-cali-0005', 23),
        
        -- Almonds
        ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'bd1-prot-0001', 21),
        ('a5b6c7d8-e9f0-4a5b-2c3d-5e6f7a8b9c0d', 'bd1-cali-0005', 579),
        
        -- Egg (whole)
        ('e160f270-05d7-4775-b9ca-5d0e896b647e', 'bd1-prot-0001', 13),
        ('e160f270-05d7-4775-b9ca-5d0e896b647e', 'bd1-fats-0003', 11),
        ('e160f270-05d7-4775-b9ca-5d0e896b647e', 'bd1-cali-0005', 155),
        
        -- Whole Wheat Bread
        ('fdbf9c99-427f-47c2-b74e-84b08077bd4d', 'bd1-carb-0002', 41),
        ('fdbf9c99-427f-47c2-b74e-84b08077bd4d', 'bd1-prot-0001', 13),
        ('fdbf9c99-427f-47c2-b74e-84b08077bd4d', 'bd1-fats-0003', 4),
        ('fdbf9c99-427f-47c2-b74e-84b08077bd4d', 'bd1-cali-0005', 252),
        
        -- Quinoa (cooked)
        ('220307e1-39b9-45e5-b827-1ce425571e40', 'bd1-carb-0002', 21),
        ('220307e1-39b9-45e5-b827-1ce425571e40', 'bd1-prot-0001', 4),
        ('220307e1-39b9-45e5-b827-1ce425571e40', 'bd1-fats-0003', 2),
        ('220307e1-39b9-45e5-b827-1ce425571e40', 'bd1-cali-0005', 120),
        
        -- Sweet Potato
        ('f24940ac-629a-46f4-9343-447d03e9fd52', 'bd1-carb-0002', 20),
        ('f24940ac-629a-46f4-9343-447d03e9fd52', 'bd1-cali-0005', 86),
        
        -- Greek Yogurt
        ('dff4517c-48eb-4dc3-aa56-5754f97d4131', 'bd1-prot-0001', 10),
        ('dff4517c-48eb-4dc3-aa56-5754f97d4131', 'bd1-cali-0005', 59),
        
        -- Cheddar Cheese
        ('5c43ecc0-9f9d-40b7-bbd1-1f401cdb2b20', 'bd1-prot-0001', 25),
        ('5c43ecc0-9f9d-40b7-bbd1-1f401cdb2b20', 'bd1-fats-0003', 33),
        ('5c43ecc0-9f9d-40b7-bbd1-1f401cdb2b20', 'bd1-cali-0005', 402),
        
        -- Carrots
        ('2541c962-a321-4210-a3d3-55d9771664d5', 'bd1-carb-0002', 10),
        ('2541c962-a321-4210-a3d3-55d9771664d5', 'bd1-cali-0005', 41),
        
        -- Rolled Oats
        ('b07683ca-62d5-43a2-a06a-98cbe170394c', 'bd1-carb-0002', 66),
        ('b07683ca-62d5-43a2-a06a-98cbe170394c', 'bd1-prot-0001', 17),
        ('b07683ca-62d5-43a2-a06a-98cbe170394c', 'bd1-fats-0003', 7),
        ('b07683ca-62d5-43a2-a06a-98cbe170394c', 'bd1-cali-0005', 379),
        
        -- Lentils (cooked)
        ('34a8dd89-d4fa-455a-9212-008a0c2c34ad', 'bd1-prot-0001', 9),
        ('34a8dd89-d4fa-455a-9212-008a0c2c34ad', 'bd1-carb-0002', 20),
        ('34a8dd89-d4fa-455a-9212-008a0c2c34ad', 'bd1-cali-0005', 116),
        
        -- Avocado
        ('025c7c9c-2d98-493e-ac8f-e51730bc44f5', 'bd1-fats-0003', 15),
        ('025c7c9c-2d98-493e-ac8f-e51730bc44f5', 'bd1-carb-0002', 9),
        ('025c7c9c-2d98-493e-ac8f-e51730bc44f5', 'bd1-cali-0005', 160);



-- Insert Supplements
INSERT INTO supplements (`SupplementId`, `Name`, `Type`, `Price`, `Stock`)
VALUES ('c7d8e9f0-a1b2-4c5d-4e5f-7a8b9c0d1e2f', 'Whey Protein Powder', 'Protein', 29.99, 150),
       ('d8e9f0a1-b2c3-4d5e-5f6a-8b9c0d1e2f3a', 'Multivitamin Complex', 'Vitamins', 19.99, 200),
       ('e9f0a1b2-c3d4-4e5f-6a7b-9c0d1e2f3a4b', 'Omega-3 Fish Oil', 'Fatty Acids', 24.99, 100),
       ('f0a1b2c3-d4e5-4f5a-7b8c-0d1e2f3a4b5c', 'Vitamin D3', 'Vitamin', 12.99, 175),
       ('a1b2c3d4-e5f6-4a5b-8c9d-1e2f3a4b5c6d', 'Calcium + Magnesium', 'Minerals', 15.99, 80),
       ('b2c3d4e5-f6a7-4b5c-9d0e-2f3a4b5c6d7e', 'Probiotic Complex', 'Digestive', 22.99, 60),
       ('c3d4e5f6-a7b8-4c5d-0e1f-3a4b5c6d7e8f', 'Iron Supplement', 'Mineral', 9.99, 120),
       ('d4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', 'BCAA Powder', 'Amino Acids', 27.99, 0),
       ('e5f6a7b8-c9d0-4e5f-2a3b-5c6d7e8f9a0b', 'Creatine Monohydrate', 'Performance', 21.99, 95),
       ('f6a7b8c9-d0e1-4f5a-3b4c-6d7e8f9a0b1c', 'Vitamin C 1000mg', 'Vitamin', 14.99, 220),
       ('a7b8c9d0-e1f2-4a5b-4c5d-7e8f9a0b1c2d', 'Magnesium Glycinate', 'Mineral', 18.99, 45),
       ('b8c9d0e1-f2a3-4b5c-5d6e-8f9a0b1c2d3e', 'Collagen Peptides', 'Protein', 32.99, 75),
       ('c9d0e1f2-a3b4-4c5d-6e7f-9a0b1c2d3e4f', 'Zinc Picolinate', 'Mineral', 11.99, 130),
       ('d0e1f2a3-b4c5-4d5e-7f8a-0b1c2d3e4f5a', 'Pre-Workout Energy', 'Performance', 28.99, 30),
       ('e1f2a3b4-c5d6-4e5f-8a9b-1c2d3e4f5a6b', 'Ashwagandha Extract', 'Herbal', 19.99, 55),
       ('f2a3b4c5-d6e7-4f5a-9b0c-2d3e4f5a6b7c', 'Beta-Alanine Powder', 'Amino Acids', 23.99, 0),
       ('a3b4c5d6-e7f8-4a5b-0c1d-3e4f5a6b7c8d', 'Turmeric Curcumin', 'Anti-Inflammatory', 26.99, 85),
       ('b4c5d6e7-f8a9-4b5c-1d2e-4f5a6b7c8d9e', 'Electrolyte Mix', 'Hydration', 16.99, 160);

-- 1. Daily Goals
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd1-prot-0001', 'Protein', 'g', 150),
       ('bd1-carb-0002', 'Carbohydrates', 'g', 250),
       ('bd1-fats-0003', 'Fats', 'g', 65),
       ('bd1-watr-0004', 'Water', 'ml', 2500),
       ('bd1-cali-0005', 'Calories', 'g', 2000);

-- 2. Minerals
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd2-calc-0001', 'Calcium', 'mg', 1000),
       ('bd2-iron-0002', 'Iron', 'mg', 18),
       ('bd2-magn-0003', 'Magnesium', 'mg', 400),
       ('bd2-phos-0004', 'Phosphorus', 'mg', 700),
       ('bd2-pota-0005', 'Potassium', 'mg', 3500),
       ('bd2-sodi-0006', 'Sodium', 'mg', 2300),
       ('bd2-zinc-0007', 'Zinc', 'mg', 11),
       ('bd2-copp-0008', 'Copper', 'mg', 0.9),
       ('bd2-mang-0009', 'Manganese', 'mg', 2.3),
       ('bd2-sele-0010', 'Selenium', 'μg', 55),
       ('bd2-iodi-0011', 'Iodine', 'μg', 150);

-- 3. Cholesterol
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd3-totl-0001', 'Cholesterol Total', 'mg/dL', 200),
       ('bd3-hdlc-0002', 'HDL Cholesterol', 'mg/dL', 40),
       ('bd3-ldlc-0003', 'LDL Cholesterol', 'mg/dL', 100),
       ('bd3-trig-0004', 'Triglycerides', 'mg/dL', 150);

-- 4. Basic Stats 
INSERT INTO `bodyStats` (`BodyStatId`, `Label`, `Unit`, `Goal`)
VALUES ('bd4-fatp-0001', 'Body Fat', '%', 25),
       ('bd4-weig-0002', 'Weight', 'kg', 75.0),
       ('bd4-hydr-0003', 'Hydration', '%', 100.0),
       ('bd4-hear-0004', 'Resting Heart Rate', 'bpm', 100.0),
       ('bd4-bmi-0005', 'BMI', '', 25.0),
       ('bd4-blood-0006', 'Blood Pressure', 'mmHg', 120.0),
       ('bd4-glucose-0007', 'Fasting Blood Glucose', 'mg/dL', 100.0);



INSERT INTO `analyse` (`AnalyseId`, `AdrianId`, `DateTime`)
VALUES ('an1-aaaa-bbbb-cccc-dddddddddddd', 'd4e5f6a7-b8c9-4d5e-1f2a-4b5c6d7e8f9a', '2025-11-18 08:30:00');

INSERT INTO `healthAnalyse` (`AnalyseId`, `BodyStatId`, `Current`)
VALUES
        -- 1. Daily Goals
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-prot-0001', 0),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-carb-0002', 0),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-fats-0003', 0),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-watr-0004', 0),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd1-cali-0005', 0),
        -- 2. Minerals
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-calc-0001', 1000),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-iron-0002', 18),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-magn-0003', 400),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-phos-0004', 700),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-pota-0005', 3500),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-sodi-0006', 2300),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-zinc-0007', 11),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-copp-0008', 1),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-mang-0009', 2.2),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-sele-0010', 55),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd2-iodi-0011', 150),
        -- 3. Cholesterol
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-totl-0001', 180),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-hdlc-0002', 50),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-ldlc-0003', 50),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd3-trig-0004', 80),
        -- 4. Basic Stats
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-fatp-0001', 15),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-weig-0002', 75),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-hydr-0003', 85),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-hear-0004', 75),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-bmi-0005', 20),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-blood-0006', 95),
        ('an1-aaaa-bbbb-cccc-dddddddddddd', 'bd4-glucose-0007', 88);
