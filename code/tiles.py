import pygame
from tools import *

class Tile():
    def __init__(self, main, pos, image, offset = pygame.math.Vector2(0, 0)):
        self.main = main
        self.image = image
        self.rect = pygame.FRect(pos.x + offset.x, pos.y + offset.y, self.image.get_width(), self.image.get_height())

    def draw(self, display):
        display.blit(self.image, self.rect.topleft - self.main.scroll)

class EnemyBoundary(Tile):
    def __init__(self, main, pos, image, offset = pygame.math.Vector2(-1, -1)):
        super().__init__(main, pos, image, offset)
        self.rect = pygame.FRect(pos.x + offset.x, pos.y + offset.y, self.image.get_width() + 2, self.image.get_height() + 2)