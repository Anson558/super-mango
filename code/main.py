import pygame, sys
from tools import load_image, load_cut_image, load_images
from entities import *
from tiles import *
from levels import levels

class Main():
    def __init__(self):
        pygame.init()

        pygame.display.set_caption('Super Mango')
        pygame.display.set_icon(load_image('star.png'))

        self.display_size = pygame.math.Vector2(320, 180)
        self.screen_size = self.display_size * 3.5
        self.screen = pygame.display.set_mode(self.screen_size)
        self.display = pygame.Surface(self.display_size)

        self.tile_size = 16

        self.level_number = 1
        self.current_level = levels[str(self.level_number)]

        self.terrain_tiles = []
        self.terrain_images = load_cut_image('grass_tiles.png')

        self.spikes_tiles = []
        self.spikes_images = load_images(['spikes.png', 'spike_block.png'])

        self.bouncepad_tiles = []
        self.bouncepad_image = load_image('bouncepad.png')

        self.star_tiles = []
        self.star_image = load_image('star.png')

        self.players = []

        self.create_groups([self.current_level['stars'], self.current_level['players'], self.current_level['bouncepads'], self.current_level['spikes'], self.current_level['terrain']])
        self.tiles = self.terrain_tiles + self.spikes_tiles + self.bouncepad_tiles + self.star_tiles
        self.entities = self.players

        self.background_image = load_image('forest_bg.png')

        menu_font = pygame.font.SysFont('images/font.ttf', 15)
        self.menu_text = menu_font.render('Click Enter to Start', False, (255, 255, 255))

        self.music = pygame.mixer.Sound('audio/music.mp3')
        self.music.play(1000)
        self.start_sound = pygame.mixer.Sound('audio/start.wav')
        self.start_sound.set_volume(0.2)

        self.scroll = pygame.math.Vector2(0, 0)

        self.state = 'menu'
        self.logo = load_image('logo.png')

        self.clock = pygame.time.Clock()

    def create_groups(self, layouts):
        self.terrain_tiles.clear()
        self.spikes_tiles.clear()
        self.bouncepad_tiles.clear()
        self.star_tiles.clear()
        self.players.clear()

        for layout in layouts:
            for row_index, row in enumerate(layout): 
                    for col_index, col in enumerate(row):
                        x = col_index * self.tile_size
                        y = row_index * self.tile_size
                        if col != '-1':
                            if layout == self.current_level['terrain']:
                                self.terrain_tiles.append(Tile(self, pygame.math.Vector2(x, y), self.terrain_images[int(col)]))
                            if layout == self.current_level['spikes']:
                                self.spikes_tiles.append(Tile(self, pygame.math.Vector2(x, y), self.spikes_images[int(col)], pygame.math.Vector2(0, 10)))
                            if layout == self.current_level['bouncepads']:
                                self.bouncepad_tiles.append(Tile(self,      pygame.math.Vector2(x, y), self.bouncepad_image, pygame.math.Vector2(0, 10)))
                            if layout == self.current_level['stars']:
                                self.star_tiles.append(Tile(self, pygame.math.Vector2(x, y), self.star_image, pygame.math.Vector2(0, 0)))
                            if layout == self.current_level['players']:
                                self.players.append(Player(self, pygame.math.Vector2(x, y)))

    def apply_scroll(self):
        for player in self.players:
            self.scroll.x += (player.rect.centerx - self.display.get_width()/2 - self.scroll.x)/10
            self.scroll.y += (player.rect.centery - self.display.get_height()/2 - self.scroll.y)/10

    def update(self):
        while True:
            self.display.blit(self.background_image, pygame.math.Vector2(0, 0))

            keys = pygame.key.get_pressed()
            for event in pygame.event.get():
                    if event.type == pygame.QUIT:
                        pygame.quit()
                        sys.exit()

            if self.state == 'game':
                if keys[pygame.K_ESCAPE]:
                    self.state = 'menu'
                    self.level_number = 1
                    self.current_level = levels[str(self.level_number)]
                    self.create_groups([self.current_level['stars'], self.current_level['players'], self.current_level['bouncepads'], self.current_level['spikes'], self.current_level['terrain']])
                    for player in self.players:
                        player.reset()

                self.tiles = self.terrain_tiles + self.spikes_tiles + self.bouncepad_tiles + self.star_tiles
                self.entities = self.players

                for player in self.players:
                    player.update()    
                    player.draw(self.display)
                    if player.level_complete == True:
                        self.level_number += 1
                        if self.level_number <= len(levels):
                            self.current_level = levels[str(self.level_number)]
                            self.create_groups([self.current_level['stars'], self.current_level['players'], self.current_level['bouncepads'], self.current_level['spikes'], self.current_level['terrain']])
                        if self.level_number > len(levels):
                            self.level_number = 1
                            self.current_level = levels[str(self.level_number)]
                            self.create_groups([self.current_level['stars'], self.current_level['players'], self.current_level['bouncepads'], self.current_level['spikes'], self.current_level['terrain']])

                        player.level_complete = False

                for tile in self.tiles:
                    tile.draw(self.display)

                self.apply_scroll()

            if self.state == 'menu':
                self.display.blit(pygame.transform.scale(self.logo, (self.logo.get_width() * 2, self.logo.get_height() * 2)), pygame.math.Vector2(self.display_size.x/2 - self.logo.get_width(), 20))
                self.display.blit(self.menu_text, pygame.math.Vector2(self.display_size.x/2 - self.menu_text.get_width()/2, 110))

                if keys[pygame.K_RETURN]:
                    self.start_sound.play()
                    self.state = 'game'

            self.screen.blit(pygame.transform.scale(self.display, self.screen_size), pygame.math.Vector2(0, 0))
            self.clock.tick(60)
            pygame.display.update()

Main().update()