// This Document contains the code for the UseLocation enum
// This enum contains the places where a Mentor can be used 
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// Mentors are very diverse so they can be used in many locations, here are the locations
public enum UseLocation
{
  Initial,            // Mentors that have this location will before start of the scoring
  Retrigger,          // Mentors that have this location will be used to set retrigger on scored cards
  PreCard,            // Mentors that have this location will be used before the card is played
  PlayCard,           // Mentors that have this location will be used during the card play phase (might not be used)
  PostCard,           // Mentors that have this location will be used after the card is played
  PreFromDraw,        // Mentors that have this location will be used before the card is drawn
  PostFromDraw,       // Mentors that have this location will be used after the card is drawn
  Post,               // Mentors that have this location will be used after the hand is played
  PreShop,            // Mentors that have this location will be used before the shop
  Shop,               // Mentors that have this location will be used in the shop
  Blind,              // Mentors that have this location will be used during the blind phase
  PostBlind,          // Mentors that have this location will be used after the blind phase
  PostHand,            // Mentors that have this location will be used after the hand is played
  AllCards            // Mentors that have this location call before selected scored PCards of hand
}
