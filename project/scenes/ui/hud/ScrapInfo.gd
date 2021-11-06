extends Control

onready var __count_label = $ScrapCountLabel

func set_scrap_count(count):
	__count_label.text = "x" + str(count)
