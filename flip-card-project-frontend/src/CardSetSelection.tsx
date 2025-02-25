import React, { useState, useEffect } from 'react';
import {Link, useLocation, useParams, To} from 'react-router-dom';
import './CardSetSelection.css';
import AddCardSet from './AddCardSet';
import { Errors } from "./errorEnums";

interface CardAttribute {
    id: number;
    question: string;
    concept: string;
    mnemonic: string;
}

interface CardSet {
    id: number;
    userId: number;
    name: string;
    flipcardsList: CardAttribute[];
}

const CardSetSelection: React.FC = () => {
    const [cardSets, setCardSets] = useState<CardSet[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const location = useLocation();
    const { userId } = location.state || {};
    const [activePlayers, setActivePlayers] = React.useState<number>(0);

    useEffect(() => {

        if (userId === 0) {
            setError('Invalid user ID');
            return;
        }
        const fetchCardSets = async () => {
            setLoading(true);
            setError(null);
            try {
                const response = await fetch(`https://localhost:44372/api/Home/${userId}/GetAllSets`);
                if (!response.ok) {
                    throw new Error(Errors.NETWORK);
                }
                const data: CardSet[] = await response.json();
                setCardSets(data);
            } catch (error) {
                setError(Errors.SETS + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCardSets();
    }, [location, userId]);

    const handleAddCardSet = (id: number, setName: string) => {
        const newCardSet: CardSet = {
            id: id,
            userId: userId,
            name: setName,
            flipcardsList: [],
        };
        setCardSets([...cardSets, newCardSet]);
    };

    const fetchActivePlayers = async () => {
        try {
            const response = await fetch('https://localhost:44372/api/Home/ActivePlayerCount');
            if (!response.ok) {
                throw new Error('Failed to fetch active players count.');
            }
            const count = await response.json();
            setActivePlayers(count);
        } catch (error) {
            console.error(`Error fetching active players: ${(error as Error).message}`);
        }
    };

    React.useEffect(() => {
        fetchActivePlayers();
        const interval = setInterval(fetchActivePlayers, 500);
        return () => clearInterval(interval);
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="cardset-selection-page">
            {<div className="active-players-count">
                Active Quiz Players: {activePlayers}
            </div>}
            <div className="card-set-selection-container">
                <div className="card-set-list">
                    {Array.isArray(cardSets) && cardSets.length > 0 ? (
                        cardSets.map((cardSet) => (
                            <div key={cardSet.id} className="card-set">
                                <Link to="/sets/set" state={{userId: userId, id: cardSet.id}}>
                                    <div className="card">
                                        <h2>{cardSet.name}</h2>
                                    </div>
                                </Link>
                            </div>
                        ))
                    ) : (
                        <div>No card sets available</div>
                    )}
                </div>
                <div className="add-card-set-form">
                    <AddCardSet userId={userId} onAdd={handleAddCardSet}/>
                </div>
            </div>
        </div>
            );
            };

            export default CardSetSelection;
