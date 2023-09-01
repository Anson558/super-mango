import json
from tools import load_csv

class Level():
    def __init__(self, level_string):
        self.level_string = level_string

        self.enemy_boundaries = load_csv(self.level_string + '/' + self.level_string + '_enemy_boundaries.csv')

        self.players = load_csv(self.level_string + '/' + self.level_string + '_player.csv')

        self.terrain = load_csv(self.level_string + '/' + self.level_string + '_grass.csv')

        self.spikes = load_csv(self.level_string + '/' + self.level_string + '_spikes.csv')

        self.bouncepads = load_csv(self.level_string + '/' + self.level_string + '_bouncepads.csv')

        self.stars = load_csv(self.level_string + '/' + self.level_string + '_stars.csv')

        self.spiders = load_csv(self.level_string + '/' + self.level_string + '_spiders.csv')

levels = {
    '1': Level('one'),
    '2': Level('two'),
    '3': Level('three'),
    '4': Level('four'),
    '5': Level('five'), 
}