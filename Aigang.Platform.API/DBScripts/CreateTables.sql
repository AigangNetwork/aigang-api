CREATE DATABASE  IF NOT EXISTS `aigang.insurance` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE `aigang.insurance`;

-- ------------------------------------------------------
-- Server version	5.6.39.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `policy`
--

DROP TABLE IF EXISTS `policy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `policy` (
  `Id` varchar(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  `DeviceId` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Status` int(11) NOT NULL DEFAULT '1',
  `Premium` decimal(12,6) NOT NULL,
  `Payout` decimal(12,6) NOT NULL,
  `Fee` decimal(12,6) NOT NULL,
  `Properties` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreateUtc` datetime NOT NULL,
  `ModifiedUtc` datetime NOT NULL,
  `PayoutUtc` datetime DEFAULT NULL,
  `ProductAddress` varchar(42) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ClaimProperties` mediumtext COLLATE utf8mb4_unicode_ci,
  `ProductTypeId` int(11) NOT NULL,
  `ClaimTx` varchar(66) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `AddPolicyTx` varchar(66) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;



--
-- Table structure for table `policyhistory`
--

DROP TABLE IF EXISTS `policyhistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `policyhistory` (
  `Id` varchar(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  `DateUtc` datetime NOT NULL,
  `StatusId` int(11) NOT NULL,
  KEY `IX_Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


CREATE DEFINER=CURRENT_USER TRIGGER `aigang.insurance`.`policy_AFTER_UPDATE` AFTER UPDATE ON `policy` FOR EACH ROW
BEGIN
   INSERT INTO `aigang.insurance`.`policyhistory`
(`Id`,
`DateUtc`,
`StatusId`)
VALUES
(NEW.Id,
NOW(),
NEW.Status);

END

--
-- Table structure for table `policystatus`
--

DROP TABLE IF EXISTS `policystatus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `policystatus` (
  `Id` int(11) NOT NULL,
  `Name` varchar(45) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `policystatus`
--

LOCK TABLES `policystatus` WRITE;
/*!40000 ALTER TABLE `policystatus` DISABLE KEYS */;
INSERT INTO `policystatus` VALUES (0,'NotSet'),(1,'Draft'),(2,'Paid'),(3,'Claimable'),(4,'PaidOut'),(5,'Canceled');
/*!40000 ALTER TABLE `policystatus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'aigang.insurance'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
