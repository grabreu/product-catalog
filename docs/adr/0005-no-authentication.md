# No authentication

There's no login, no accounts, no auth of any kind — every request acts as a single implicit user. This is deliberate: the point of this project is architecture, testing, and deployment rigor around a simple domain, and auth would add complexity that distracts from that without teaching anything new. Not a gap to fill later; revisit only if this ever needs to model real users or ownership.
