import pygame
from tools import *

class Entity():
    def __init__(self, main, pos, image):
        pygame.init()
        self.default_pos = pos
        self.main = main

        self.image = image
        self.rect = pygame.FRect(self.default_pos.x, self.default_pos.y, self.image.get_width(), self.image.get_height())

        self.jump_sound = pygame.mixer.Sound('audio/jump.wav')
        self.jump_sound.set_volume(0.2)

        self.velocity = pygame.math.Vector2(0, 0)
        self.gravity = 0.15
        self.flip = False
        self.air_time = 0

    def update(self):
        self.rect.x += self.velocity.x
        for tile in self.main.terrain_tiles:
            if self.rect.colliderect(tile):
                if self.velocity.x > 0:
                    self.rect.right = tile.rect.left
                elif self.velocity.x < 0:
                    self.rect.left = tile.rect.right
                    self.velocity.y += self.gravity

        self.air_time += 1
        self.rect.y += self.velocity.y
        self.velocity.y += self.gravity
        for tile in self.main.terrain_tiles:
            if self.rect.colliderect(tile):
                if self.velocity.y > 0:
                    self.air_time = 0
                    self.rect.bottom = tile.rect.top
                    self.velocity.y = 0
                elif self.velocity.y < 0:
                    self.rect.top = tile.rect.bottom
                    self.velocity.y = 0

        for tile in self.main.bouncepad_tiles:
            if self.rect.colliderect(tile):
                self.jump_sound.play()
                self.velocity.y = -6

    def reset(self):
        self.rect.topleft = self.default_pos
        self.velocity.y = 0

    def draw(self, display):
        display.blit(pygame.transform.flip(self.image, self.flip, False), self.rect.topleft - self.main.scroll)

class Player(Entity):
    def __init__(self, main, pos):
        super().__init__(main, pos, load_cut_image('player_idle.png')[0])

        self.rect = pygame.FRect(pos.x, pos.y, self.image.get_width(), self.image.get_height())

        self.die_sound = pygame.mixer.Sound('audio/die.wav')
        self.die_sound.set_volume(0.8)
        self.level_complete_sound = pygame.mixer.Sound('audio/level_complete.wav')
        self.level_complete_sound.set_volume(0.7)

        self.speed = 1.5
        self.jump_height = -4

        self.level_complete = False

        self.idle_anim = Animation(load_cut_image('player_idle.png'), 15)
        self.run_anim = Animation(load_cut_image('player_run.png'), 8)
        self.jump_anim = Animation(load_cut_image('player_jump.png'), 1)
        self.fall_anim = Animation(load_cut_image('player_fall.png'), 1)

    def update(self):
        super().update()

        self.move()
        self.animate()

        danger_tiles = self.main.spikes_tiles + self.main.spiders
        for tile in danger_tiles:
            if self.rect.colliderect(tile.rect):
                self.die_sound.play()
                self.reset()

        win_tiles = self.main.star_tiles
        for tile in win_tiles:
            if self.rect.colliderect(tile):
                self.level_complete_sound.play()
                self.level_complete = True

        if self.rect.y > 600:
            self.die_sound.play()
            self.reset()

    def move(self):
        keys = pygame.key.get_pressed()

        if keys[pygame.K_d] or keys[pygame.K_RIGHT]:
            self.velocity.x = 1 * self.speed
            self.flip = False
        elif keys[pygame.K_a] or keys[pygame.K_LEFT]:
            self.velocity.x = -1 * self.speed
            self.flip = True
        else:
            self.velocity.x = 0

        if keys[pygame.K_SPACE]:
            if self.air_time < 1:
                self.jump_sound.play()
                self.velocity.y = self.jump_height
                
    def animate(self):
        animations = self.idle_anim, self.run_anim, self.jump_anim, self.fall_anim
        
        for animation in animations:
            animation.play()

        if self.velocity.x > 0:
            self.image = self.run_anim.image
            self.flip = False
        if self.velocity.x < 0:
            self.image = self.run_anim.image
            self.flip = True
        if self.velocity.x == 0:
            self.image = self.idle_anim.image

        if self.velocity.y < 0:
            self.image = self.jump_anim.image
        if self.velocity.y > self.gravity:
            self.image = self.fall_anim.image

class Spider(Entity):
    def __init__(self, main, pos):
        super().__init__(main, pos, load_cut_image('spider_run.png')[0])
        self.speed = 1
        print(self.speed)
        self.rect = pygame.FRect(self.default_pos.x, self.default_pos.y, self.image.get_width(), self.image.get_height())
        self.animation = Animation(load_cut_image('spider_run.png'), 8)
        self.velocity.x = -self.speed

    def update(self):
        super().update()
        self.animate()

        for tile in self.main.enemy_boundaries:
            if self.rect.colliderect(tile):
                self.velocity.x *= -1

    def animate(self):
        self.animation.play()
        self.image = self.animation.image

        if self.velocity.x > 0:
            self.flip = True
        if self.velocity.x < 0:
            self.flip = False
