Projeto de Portais 3D em Unity
Este projeto demonstra um sistema de portais 3D funcionais na Unity, inspirado em jogos como Portal. O sistema permite ao jogador ver e atravessar portais em tempo real, viajando entre três salas interconectadas.

🔮 Funcionalidades
3 Salas: Verde (A), Laranja (B) e Azul (C).

Portais de Dupla Face: Cada sala possui um portal com duas faces, permitindo viagens complexas.

Circuito Completo (A-B-C):

Sala A (Verde): Frente -> Laranja (B) | Trás -> Azul (C)

Sala B (Laranja): Frente -> Verde (A) | Trás -> Azul (C)

Sala C (Azul): Frente -> Verde (A) | Trás -> Laranja (B)

Visualização em Tempo Real: A vista através de cada portal é uma transmissão ao vivo da sala de destino.

Teletransporte Contínuo: O jogador é teletransportado de forma fluida ao atravessar o sensor do portal.

Mecânica de Escala Dinâmica: O jogador "encolhe" (divide a altura da câmara por 2) ao atravessar o portal pela frente e "cresce" (dobra a altura) ao atravessar por trás, com limites mínimo e máximo.

⚙️ Implementação Técnica
Efeito Visual (Render Texture)
Cada sala possui uma Camera que filma o ambiente. Essa filmagem é enviada para uma Render Texture (uma "fita de vídeo"). Três Materials (materiais) independentes são usados para "sintonizar" esses feeds de vídeo. Cada face do portal (um Quad) recebe o material correspondente à sua sala de destino.

Lógica de Teletransporte (PortalMestreTeleporter.cs)
Um script único é anexado a um "Sensor" invisível (Box Collider com Is Trigger) em cada portal.

O script possui dois destinos (destination_LadoFrente, destination_LadoTras).

Ele deteta de que lado o jogador entra e para que lado ele atravessa (verificando a mudança na posição Z local).

Ao cruzar, o script teletransporta o Player para o sensor de destino correto.

O script também gere a lógica de escala, alterando o localPosition.y da câmara do jogador a cada travessia (dividindo ou multiplicando a altura) e aplicando os limites minCameraHeight e maxCameraHeight para controlar o efeito.