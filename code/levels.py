import json
from tools import load_level

levels = {
    '1': {
            'players': load_level('one/one_player.csv'),

            'terrain': load_level('one/one_grass.csv'),

            'spikes': load_level('one/one_spikes.csv'),

            'bouncepads': load_level('one/one_bouncepads.csv'),

            'stars': load_level('one/one_stars.csv'),
    },

    '2': {
            'players': load_level('two/two_player.csv'),

            'terrain': load_level('two/two_grass.csv'),

            'spikes': load_level('two/two_spikes.csv'),

            'bouncepads': load_level('two/two_bouncepads.csv'),

            'stars': load_level('two/two_stars.csv'),
    }
}