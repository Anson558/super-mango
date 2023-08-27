import pygame
from csv import reader
import os

base_img_path = 'images/'
base_level_path = 'levels/'
tile_size = 16

def load_image(path):
    img = pygame.image.load(base_img_path + path)
    return img

def load_images(paths):
	imgs = []
	for path in paths:
		imgs.append(load_image(path))

	return imgs

def load_level(path):
	terrain_map = []
	with open(base_level_path +  path) as map:
		level = reader(map,delimiter = ',')
		for row in level:
			terrain_map.append(list(row))
		return terrain_map

def load_cut_image(path):
	surface = load_image(path)
	tile_num_x = int(surface.get_size()[0] / tile_size)
	tile_num_y = int(surface.get_size()[1] / tile_size)

	cut_tiles = []
	for row in range(tile_num_y):
		for col in range(tile_num_x):
			x = col * tile_size
			y = row * tile_size
			new_surf = pygame.Surface((tile_size,tile_size),flags = pygame.SRCALPHA)
			new_surf.blit(surface,(0,0),pygame.Rect(x,y,tile_size,tile_size))
			cut_tiles.append(new_surf)

	return cut_tiles

class Animation:
	def __init__(self, images, image_duration):
		self.images = images
		self.frame = 0
		self.image = self.images[self.frame]
		self.image_duration = image_duration
		self.time_since_switch = 0

	def play(self):
		self.time_since_switch += 1

		self.image = self.images[self.frame]

		if self.time_since_switch > self.image_duration:
			self.time_since_switch = 0
			if self.frame < len(self.images) - 1:
				self.frame += 1
			else:
				self.frame = 0
