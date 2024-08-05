-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: hamster_data
-- ------------------------------------------------------
-- Server version	8.0.31

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cards`
--

DROP TABLE IF EXISTS `cards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cards` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `profit` int NOT NULL DEFAULT '0',
  `cost` int NOT NULL DEFAULT '0',
  `image_url` varchar(45) DEFAULT NULL,
  `category` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cards`
--

LOCK TABLES `cards` WRITE;
/*!40000 ALTER TABLE `cards` DISABLE KEYS */;
INSERT INTO `cards` VALUES (1,'defaultCard',100,1000,'mining.jpg','PR'),(2,'bestCard',2000,1000,'mining2.jpg','Special'),(3,'Legal issues',600,5000,'mining3.jpg','Legal'),(4,'License bangladesh',400,100000,'mining4.jpg','Legal'),(5,NULL,8000,54000,'mining5.jpg',NULL);
/*!40000 ALTER TABLE `cards` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ranks`
--

DROP TABLE IF EXISTS `ranks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ranks` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `coins_required` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ranks`
--

LOCK TABLES `ranks` WRITE;
/*!40000 ALTER TABLE `ranks` DISABLE KEYS */;
INSERT INTO `ranks` VALUES (1,'Bronze',0),(2,'Silver',5000),(3,'Gold',10000),(4,'Platinum',100000);
/*!40000 ALTER TABLE `ranks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_ranks`
--

DROP TABLE IF EXISTS `user_ranks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_ranks` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL DEFAULT '0',
  `rank_id` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_ranks`
--

LOCK TABLES `user_ranks` WRITE;
/*!40000 ALTER TABLE `user_ranks` DISABLE KEYS */;
INSERT INTO `user_ranks` VALUES (1,52,1);
/*!40000 ALTER TABLE `user_ranks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usercards`
--

DROP TABLE IF EXISTS `usercards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usercards` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL DEFAULT '0',
  `card_id` int NOT NULL DEFAULT '0',
  `current_level` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usercards`
--

LOCK TABLES `usercards` WRITE;
/*!40000 ALTER TABLE `usercards` DISABLE KEYS */;
INSERT INTO `usercards` VALUES (1,52,1,1),(2,52,1,1),(3,52,1,1),(4,52,1,1),(5,52,1,1),(6,52,1,1),(7,52,1,1),(8,0,1,1),(9,0,1,1),(10,0,3,1),(11,0,1,1),(12,0,1,1),(13,0,1,1),(14,0,1,1),(15,0,1,1),(16,0,1,1),(17,0,1,1),(18,0,1,1),(19,0,1,1),(20,0,1,1),(21,0,1,1),(22,0,1,1),(23,0,1,1),(24,0,1,1),(25,0,1,1),(26,0,1,1),(27,0,1,1),(28,52,1,1),(29,52,1,1),(30,52,1,1),(31,52,1,1),(32,52,1,1),(33,52,1,1),(34,52,1,1),(35,52,1,1),(36,52,1,1),(37,52,1,1),(38,52,1,1),(39,52,1,1),(40,52,1,1),(41,52,1,1),(42,52,1,1),(43,52,4,1),(44,52,1,1),(45,52,1,1),(46,52,1,1),(47,52,1,1),(48,52,1,1),(49,52,1,1),(50,52,1,1),(51,0,1,1),(52,0,3,1);
/*!40000 ALTER TABLE `usercards` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `coin_balance` int NOT NULL DEFAULT '0',
  `profit` int NOT NULL DEFAULT '0',
  `earn_per_tap` int NOT NULL DEFAULT '8',
  `last_connected` int NOT NULL DEFAULT '0',
  `current_rank` int NOT NULL DEFAULT '1',
  `coins_to_level_up` int NOT NULL DEFAULT '5000',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'navid',16,0,8,0,1,5000),(2,'navid',40,0,8,0,1,5000),(3,'navid',8,0,8,0,1,5000),(4,'navid',0,0,8,0,1,5000),(5,'navid',0,0,8,0,1,5000),(6,'navid',0,0,8,0,1,5000),(7,'navid',0,0,8,0,1,5000),(8,'navid',80,0,8,0,1,5000),(9,'navid',2176,0,8,0,1,2832);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-08-05 19:09:19
