-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mar. 30 nov. 2021 à 10:38
-- Version du serveur : 10.4.19-MariaDB
-- Version de PHP : 8.0.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `db_evento`
--

-- --------------------------------------------------------

--
-- Structure de la table `palestrantes`
--

CREATE TABLE `palestrantes` (
  `IdPalestrante` int(11) NOT NULL,
  `Nome` varchar(80) NOT NULL,
  `E_mail` varchar(80) NOT NULL,
  `MiniCurriculum` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Structure de la table `palestras`
--

CREATE TABLE `palestras` (
  `IdPalestrante` int(11) NOT NULL,
  `IdPalestra` int(11) NOT NULL,
  `NomePalestrante` varchar(80) NOT NULL,
  `Data` date NOT NULL,
  `Tema` varchar(255) NOT NULL,
  `SHA` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Structure de la table `usuarios`
--

CREATE TABLE `usuarios` (
  `Nome` varchar(80) NOT NULL,
  `Senha` varchar(80) NOT NULL,
  `Administrador` tinyint(4) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `palestrantes`
--
ALTER TABLE `palestrantes`
  ADD PRIMARY KEY (`IdPalestrante`);

--
-- Index pour la table `palestras`
--
ALTER TABLE `palestras`
  ADD PRIMARY KEY (`IdPalestra`),
  ADD KEY `Fk_IdPalestrante` (`IdPalestrante`);

--
-- Index pour la table `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Nome`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `palestrantes`
--
ALTER TABLE `palestrantes`
  MODIFY `IdPalestrante` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=69;

--
-- AUTO_INCREMENT pour la table `palestras`
--
ALTER TABLE `palestras`
  MODIFY `IdPalestra` int(11) NOT NULL AUTO_INCREMENT;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `palestras`
--
ALTER TABLE `palestras`
  ADD CONSTRAINT `Fk_IdPalestrante` FOREIGN KEY (`IdPalestrante`) REFERENCES `palestrantes` (`IdPalestrante`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
