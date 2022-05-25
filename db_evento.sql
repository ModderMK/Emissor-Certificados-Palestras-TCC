-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mer. 20 oct. 2021 à 23:50
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
-- Structure de la table `certificados`
--

CREATE TABLE `certificados` (
  `IdPalestrante` int(11) NOT NULL,
  `IdPalestra` int(11) NOT NULL,
  `NomePalestrante` varchar(80) NOT NULL,
  `Tema` text NOT NULL,
  `DataEmissao` date NOT NULL,
  `SHA` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Déchargement des données de la table `certificados`
--

INSERT INTO `certificados` (`IdPalestrante`, `IdPalestra`, `NomePalestrante`, `Tema`, `DataEmissao`, `SHA`) VALUES
(41, 45, 'Ryan Souza Melo', 'Como começar uma carreira em programação?', '2021-10-16', '7d8e4571292d05fa7f9d5f3cb7047d8158fa48fd'),
(37, 46, 'Victor Silveira Neto', 'que tipos de projeto devo colocar no portfólio?', '2021-10-13', '97bb297ef184c499dc91f8990fc0fa62fe5757f7'),
(37, 47, 'Victor Silveira Neto', 'a importância das soft skills em programação', '2021-12-12', 'a9ebe4b4bdfb0752b24dc991d7bb16445d08d61f'),
(49, 48, 'Enrico Ferreira Rocha', 'Quais as áreas mais \"quentes\" em TI hoje em dia?', '2021-10-10', 'da39a3ee5e6b4b0d3255bfef95601890afd80709'),
(43, 49, 'Caio Visconti Nunes', 'Por qual linguagem de programação você deveria começar?', '2020-02-10', '8d6667903e6e61ed97c6685dfeb120f3c7e97ef3'),
(46, 50, 'Eduardo Correia Santos', 'Faculdade é realmente necessário para trabalhar com programação?', '2019-10-10', '9dba7da8e1a25d0a1974fd656e4ca5322502a440'),
(44, 51, 'Luísa Soares Xavier De Aguilar', 'Como ir trabalhar no exterior como dev?', '2018-04-15', 'eb296d6aef4aa55a356f86f1edb1fe8451bd9b64'),
(50, 52, 'Wallace Feliciano Silva', 'Desenvolvimento Web Hoje E Dez Anos Atrás', '2021-10-17', '3decd49a6c6dce88c16a85b9a8e42b51aa36f1e2'),
(37, 59, 'VICTOR SILVEIRA NETO', 'Asdasdsd', '2002-12-12', '591bb41a2ea6f137b6b083fa775d9c4fdf65d837');

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

--
-- Déchargement des données de la table `palestrantes`
--

INSERT INTO `palestrantes` (`IdPalestrante`, `Nome`, `E_mail`, `MiniCurriculum`) VALUES
(36, 'GIOVANA LIMA ALMEIDA', 'giovana_almeida@gmail.com', 'Descrição das experiências e formações.\n'),
(37, 'VICTOR SILVEIRA NETO', 'victorneto@gmail.com', 'Descrição das experiências e formações.\n'),
(38, 'PAULO LIMA GONÇALVEZ', 'paulogoncalvez@gmail.com', 'Descrição das experiências e formações.\n '),
(39, 'LUANA SOUZA RIBEIRO', 'luanaribeira@gmail.com', 'Descrição das experiências e formações.\n'),
(40, 'MATHEUS BARBOSA', 'matheusbarbosa@gmail.com', 'Descrição das experiências e formações.\n'),
(41, 'RYAN SOUZA MELO', 'ryanmelo@gmail.com', 'Descrição das experiências e formações.\n'),
(42, 'MARIA CLARA DOS SANTOS', 'mariasantos@gmail.com', 'Descrição das experiências e formações.\n'),
(43, 'CAIO VISCONTI NUNES', 'caionunes@gmail.com', 'Descrição das experiências e formações.\n'),
(44, 'LUÍSA SOARES XAVIER DE AGUILAR', 'luisaaguilar@outlook.com', 'Descrição das experiências e formações.\n'),
(45, 'EDUARDO ALVARENGA FILHO', 'eduardofilho@outlook.com', 'Descrição das experiências e formações.\n'),
(46, 'EDUARDO CORREIA SANTOS', 'eduardosantos@outlook.com', 'Descrição das experiências e formações.\n'),
(47, 'VICTOR ROCHA  LACERDA', 'victorlacerda@outlook.com', 'Descrição das experiências e formações.\n'),
(48, 'CAIO ALVARENGA SOUZA', 'caiosouza@outlook.com', 'Descrição das experiências e formações.\n\n'),
(49, 'ENRICO FERREIRA ROCHA', 'enricorocha@gmail.com', 'Descrição das experiências e formações.\n\n'),
(50, 'WALLACE FELICIANO SILVA', 'wallacesilva@outlook.com', 'Descrição das experiências e formações.'),
(51, 'asdasdasd', 'asdasdasd', 'asdasd');

-- --------------------------------------------------------

--
-- Structure de la table `palestras`
--

CREATE TABLE `palestras` (
  `IdPalestrante` int(11) NOT NULL,
  `IdPalestra` int(11) NOT NULL,
  `NomePalestrante` varchar(80) NOT NULL,
  `Data` date NOT NULL,
  `Tema` text NOT NULL,
  `SHA` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Déchargement des données de la table `palestras`
--

INSERT INTO `palestras` (`IdPalestrante`, `IdPalestra`, `NomePalestrante`, `Data`, `Tema`, `SHA`) VALUES
(41, 45, 'RYAN SOUZA MELO', '2021-10-16', 'Como Começar Uma Carreira Em Programação?', '7d8e4571292d05fa7f9d5f3cb7047d8158fa48fd'),
(37, 46, 'VICTOR SILVEIRA NETO', '2021-10-13', 'Que Tipos De Projeto Devo Colocar No Portfólio?', '97bb297ef184c499dc91f8990fc0fa62fe5757f7'),
(37, 47, 'VICTOR SILVEIRA NETO', '2021-12-12', 'A Importância Das Soft Skills Em Programação', 'a9ebe4b4bdfb0752b24dc991d7bb16445d08d61f'),
(49, 48, 'ENRICO FERREIRA ROCHA', '2021-10-10', 'Quais As Áreas Mais \"Quentes\" Em Ti Hoje Em Dia?', 'da39a3ee5e6b4b0d3255bfef95601890afd80709'),
(43, 49, 'CAIO VISCONTI NUNES', '2020-02-10', 'Por Qual Linguagem De Programação Você Deveria Começar?', '8d6667903e6e61ed97c6685dfeb120f3c7e97ef3'),
(46, 50, 'EDUARDO CORREIA SANTOS', '2019-10-10', 'Faculdade É Realmente Necessário Para Trabalhar Com Programação?', '9dba7da8e1a25d0a1974fd656e4ca5322502a440'),
(44, 51, 'LUÍSA SOARES XAVIER DE AGUILAR', '2018-04-15', 'Como Ir Trabalhar No Exterior Como Dev?', 'eb296d6aef4aa55a356f86f1edb1fe8451bd9b64'),
(50, 52, 'WALLACE FELICIANO SILVA', '2021-10-17', 'Desenvolvimento Web Hoje E Dez Anos Atrás', '3decd49a6c6dce88c16a85b9a8e42b51aa36f1e2'),
(37, 59, 'VICTOR SILVEIRA NETO', '2002-12-12', 'Asdasdsd', '591bb41a2ea6f137b6b083fa775d9c4fdf65d837');

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
-- Déchargement des données de la table `usuarios`
--

INSERT INTO `usuarios` (`Nome`, `Senha`, `Administrador`) VALUES
('arthur', '40bd001563085fc35165329ea1ff5c5ecbdbbeef', 1),
('arthur2', '40bd001563085fc35165329ea1ff5c5ecbdbbeef', 0);

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `certificados`
--
ALTER TABLE `certificados`
  ADD PRIMARY KEY (`IdPalestra`),
  ADD KEY `FK_certificado_palestrante_IdPalestrante` (`IdPalestrante`);

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
  MODIFY `IdPalestrante` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- AUTO_INCREMENT pour la table `palestras`
--
ALTER TABLE `palestras`
  MODIFY `IdPalestra` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=60;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `certificados`
--
ALTER TABLE `certificados`
  ADD CONSTRAINT `FK_certificado_palestrante_IdPalestrante` FOREIGN KEY (`IdPalestrante`) REFERENCES `palestrantes` (`IdPalestrante`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `Fk_certificado_palestra_IdPalestra` FOREIGN KEY (`IdPalestra`) REFERENCES `palestras` (`IdPalestra`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `palestras`
--
ALTER TABLE `palestras`
  ADD CONSTRAINT `Fk_IdPalestrante` FOREIGN KEY (`IdPalestrante`) REFERENCES `palestrantes` (`IdPalestrante`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
