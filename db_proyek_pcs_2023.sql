-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 22, 2023 at 02:50 PM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 8.0.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_proyek_pcs_2023`
--

-- --------------------------------------------------------

--
-- Table structure for table `bahan`
--

CREATE TABLE `bahan` (
  `ID_BAHAN` int(11) NOT NULL,
  `NAMA_BAHAN` varchar(255) NOT NULL,
  `STOK` varchar(255) NOT NULL,
  `SATUAN` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
INSERT INTO BAHAN VALUES(1, 'GARAM', 1000, 'GRAM');
INSERT INTO BAHAN VALUES(2, 'GULA', 10000, 'GRAM');
INSERT INTO BAHAN VALUES(3, 'NASI', 10000, 'GRAM');
INSERT INTO BAHAN VALUES(4, 'AIR', 10000, 'ML');
INSERT INTO BAHAN VALUES(5, 'KECAP', 10000, 'ML');
INSERT INTO BAHAN VALUES(6, 'BAWANG', 10000, 'GRAM');
INSERT INTO BAHAN VALUES(7, 'SAYUR', 10000, 'GRAM');
INSERT INTO BAHAN VALUES(8, 'MIE', 10000, 'GRAM');
INSERT INTO BAHAN VALUES(9, 'TEH', 10000, 'ML');
INSERT INTO BAHAN VALUES(10, 'SUSU', 10000, 'ML');

-- --------------------------------------------------------

--
-- Table structure for table `resep`
--

CREATE TABLE `resep` (
  `ID_RESEP` int(11) NOT NULL,
  `ID_FNB` INT(11) NOT NULL,
  `ID_BAHAN` INT(11) NOT NULL,
  `STOK` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
INSERT INTO RESEP VALUES(1, 1, 1, 500);
INSERT INTO RESEP VALUES(2, 1, 2, 500);

INSERT INTO RESEP VALUES(3, 2, 7, 300);

INSERT INTO RESEP VALUES(4, 3, 3, 500);
INSERT INTO RESEP VALUES(5, 3, 5, 500);

INSERT INTO RESEP VALUES(6, 4, 8, 500);
INSERT INTO RESEP VALUES(7, 4, 5, 500);

INSERT INTO RESEP VALUES(8, 5, 10, 1000);
INSERT INTO RESEP VALUES(9, 5, 2, 100);

INSERT INTO RESEP VALUES(10, 6, 4, 100);

INSERT INTO RESEP VALUES(11, 7, 4, 200);
INSERT INTO RESEP VALUES(12, 7, 2, 5);

INSERT INTO RESEP VALUES(13, 8, 4, 200);
INSERT INTO RESEP VALUES(14, 8, 1, 5);



-- --------------------------------------------------------

--
-- Table structure for table `d_trans`
--

CREATE TABLE `d_trans` (
  `NOMOR_NOTA_DTRANS` varchar(255) NOT NULL,
  `NAMA_FNB` VARCHAR(255) NOT NULL,
  `QTY` int(11) NOT NULL,
  `HARGA` int(11) NOT NULL,
  `SUBTOTAL` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `fnb`
--

CREATE TABLE `fnb` (
  `ID_FNB` int(11) NOT NULL,
  `NAMA_FNB` varchar(255) NOT NULL,
  `JENIS_FNB` varchar(255) NOT NULL,
  `ID_RESEP` int(11) NOT NULL,
  `HARGA` INT(11) NOT NULL,
  `PROMO` INT(11) NOT NULL,
  `PAKET` INT(11) NOT NULL,
  `AVAILABLE` INT(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
 INSERT INTO FNB VALUES(1, 'GARAM DAN GULA', 'MAKANAN', 1, 5000, 0, 0, 1);
 INSERT INTO FNB VALUES(2, 'SAYUR GORENG', 'MAKANAN', 2, 17000, 0, 0, 1);
 INSERT INTO FNB VALUES(3, 'NASI KECAP', 'MAKANAN', 3, 15000, 0, 0, 1);
 INSERT INTO FNB VALUES(4, 'MIE KECAP', 'MAKANAN', 4, 15000, 0, 0, 1);
 INSERT INTO FNB VALUES(5, 'SUSU', 'MINUMAN', 5, 25000, 5000, 0, 1);
 INSERT INTO FNB VALUES(6, 'AIR PUTIH', 'MINUMAN', 6, 5000, 0, 0, 1);
 INSERT INTO FNB VALUES(7, 'TEH MANIS', 'MINUMAN', 7, 7000, 0, 0, 1);
 INSERT INTO FNB VALUES(8, 'TEH ASIN', 'MINUMAN', 8, 7000, 500, 0, 1);
-----------------------------------------------------------

--
-- Table structure for table `h_trans`
--

CREATE TABLE `h_trans` (
  `NOMOR_NOTA_HTRANS` varchar(255) NOT NULL,
  `TANGGAL_TRANS` date NOT NULL,
  `TOTAL` int(11) NOT NULL,
  `STATUS` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `karyawan`
--

CREATE TABLE `karyawan` (
  `ID_KARYAWAN` int(12) NOT NULL,
  `NAMA` varchar(255) NOT NULL,
  `USERNAME` varchar(255) NOT NULL,
  `PASSWORD` varchar(255) NOT NULL,
  `JABATAN` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bahan`
--
ALTER TABLE `bahan`
  ADD PRIMARY KEY (`ID_BAHAN`);

--
-- Indexes for table `resep`
--
ALTER TABLE `resep`
  ADD PRIMARY KEY (`ID_RESEP`);

--
-- Indexes for table `d_trans`
--
ALTER TABLE `d_trans`
  ADD PRIMARY KEY (`NOMOR_NOTA_DTRANS`);

--
-- Indexes for table `fnb`
--
ALTER TABLE `fnb`
  ADD PRIMARY KEY (`ID_FNB`);

--
-- Indexes for table `h_trans`
--
ALTER TABLE `h_trans`
  ADD PRIMARY KEY (`NOMOR_NOTA_HTRANS`);

--
-- Indexes for table `karyawan`
--
ALTER TABLE `karyawan`
  ADD PRIMARY KEY (`ID_KARYAWAN`);

COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
